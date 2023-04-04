using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.Http;
using IniParser;
using IniParser.Model;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Threading.Tasks;
using DatabaseToCode.Model;
using System.Data.Common;
using System.Data.SqlTypes;
using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.Utility
{
    public class CommonFunctions
    {
        public IConfiguration Configuration { get; }
        public string ContentRootPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public CommonFunctions(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public CommonFunctions(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, string contentRootPath)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            ContentRootPath = contentRootPath;

            if (!String.IsNullOrEmpty(ContentRootPath))
            {
                var pathArray = ContentRootPath.Split("\\").ToList();
                pathArray.RemoveRange(pathArray.Count - 1, 1);
                this.ContentRootPath = String.Join("\\", pathArray) + "\\";
            }
        }

        public CommonFunctions()
        {

        }

        public TokenModel GetTokenData()
        {
            try
            {
                string token = GetTokenFromHeader();

                if(String.IsNullOrEmpty(token))
                {
                    return null;
                }

                token = token.Replace("Bearer ", "");
                var jwtSection = Configuration.GetSection("Jwt");
                var Secret = Encoding.ASCII.GetBytes(jwtSection.GetValue<String>("Secret"));
                var ValidIssuer = jwtSection.GetValue<String>("ValidIssuer");
                var ValidAudience = jwtSection.GetValue<String>("ValidAudience");
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                var otherClaims = claims.Identities.ToList()[0].Claims.ToList(); 

                TokenModel TokenData = new TokenModel();
                TokenData = JsonConvert.DeserializeObject<TokenModel>(otherClaims.Find(x => x.Type == "TokenData").Value);
                return TokenData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetTokenFromHeader()
        {
            try
            {
                return HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetDataTypeFor(TableColumn Column, string LanguageDataType)
        {
            var CustomPathsSection = Configuration.GetSection("CustomPaths");
            List<SqlDataTypeCorrespondingLanguagesModel> sqlDataTypeCorrespondingLanguages = new List<SqlDataTypeCorrespondingLanguagesModel>();
            try
            {
                var FilePath = Path.Combine(ContentRootPath, CustomPathsSection.GetValue<String>("SqlDataTypeCorrespondingLanguagesFile"));
                string JsonData = System.IO.File.ReadAllText(FilePath);
                sqlDataTypeCorrespondingLanguages = JsonConvert.DeserializeObject<List<SqlDataTypeCorrespondingLanguagesModel>>(JsonData);
            }
            catch (Exception) { }

            var DataTypeObject = sqlDataTypeCorrespondingLanguages.Find(x => x.SQLDataType == Column.DataType);

            string TOReturnDataType;

            if (DataTypeObject == null)
            {
                TOReturnDataType = "string";
            }
            else
            {
                TOReturnDataType = typeof(SqlDataTypeCorrespondingLanguagesModel).GetProperty(LanguageDataType).GetValue(DataTypeObject).ToString();
            }

            switch (LanguageDataType)
            {
                case "CSharpDataType":
                    if (TOReturnDataType.ToUpper() != "string".ToUpper() && Column.IsNullable.ToUpper() == "YES")
                    {
                        TOReturnDataType += "?";
                    }
                    break;
            }
            

            

            return TOReturnDataType;
        }

        public string GetDefaultDataFor(TableColumn Column, string LanguageDataType)
        {
            string TOReturnDefault = "= ";

            switch (LanguageDataType)
            {
                case "CSharpDataType":
                    if (Column.IsNullable.ToUpper() == "NO")
                    {
                        if(Column.DataType == "bit")
                        {
                            if(Column.ColumnDefault != null)
                            {
                                TOReturnDefault += Column.ColumnDefault == "0" ? "false;" : "true;";
                            }
                        }
                        else
                        {
                            if(Column.ColumnDefault == "(newsequentialid())")
                            {
                                TOReturnDefault += "Guid.NewGuid();";
                            }
                            else
                            {
                                TOReturnDefault += Column.ColumnDefault;
                            }
                        }
                        
                    }
                    break;
            }

            if(TOReturnDefault == "= ")
            {
                return "";
            }

            return TOReturnDefault;
        }
    }
}
