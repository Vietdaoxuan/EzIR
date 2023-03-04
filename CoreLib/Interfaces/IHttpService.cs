using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IHttpService
    {
        void SetRoleControllerToHeader(string roleCode);

        void SetUsernameControllerToHeader(string username);

        // HTTP GET
        Task<DataSet> FetchDataSetFromGetUrl(string requestUri);

        Task<DataTable> FetchDataTableFromGetUrl(string requestUri);

        Task<DataRow> FetchDataRowFromGetUrl(string requestUri);

        Task<CResponseMessage> ResponseMessageFromGetUrl(string requestUri);

        // HTTP POST
        Task<DataSet> FetchDataSetFromPostUrl(string requestUri, object data);

        Task<DataTable> FetchDataTableFromPostUrl(string requestUri, object data);

        Task<DataRow> FetchDataRowFromPostUrl(string requestUri, object data);

        Task<CResponseMessage> ResponseMessageFromPostUrl(string requestUri, object data);

        // HTTP PUT
        Task<CResponseMessage> ResponseMessageFromPutUrl(string requestUri, object data);

        // HTTP DELETE
        Task<CResponseMessage> ResponseMessageFromDeleteUrl(string requestUri);

        Task<clsWebsiteRequest> CallAPIWebsite(NewsWebsiteAPI cNewsAPI, string APIBasi, string API_Paragram);

        Task FetchDataTableFromGetUrl(object p);

        
    
    }
}
