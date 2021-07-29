using Com.Zoho.Crm.API;
using Com.Zoho.Crm.API.Util;
using System;

namespace Com.Zoho.Crm.API.RelatedRecords
{

	public class RelatedRecordsOperations
	{
		private string moduleAPIName;
		private string relatedListAPIName;
		private string xExternal;

		/// <summary>		/// Creates an instance of RelatedRecordsOperations with the given parameters
		/// <param name="relatedListAPIName">string</param>
		/// <param name="moduleAPIName">string</param>
		/// <param name="xExternal">string</param>
		
		public RelatedRecordsOperations(string relatedListAPIName, string moduleAPIName, string xExternal)
		{
			 this.relatedListAPIName=relatedListAPIName;

			 this.moduleAPIName=moduleAPIName;

			 this.xExternal=xExternal;


		}

		/// <summary>The method to get related records</summary>
		/// <param name="recordId">long?</param>
		/// <param name="paramInstance">Instance of ParameterMap</param>
		/// <param name="headerInstance">Instance of HeaderMap</param>
		/// <returns>Instance of APIResponse<ResponseHandler></returns>
		public APIResponse<ResponseHandler> GetRelatedRecords(long? recordId, ParameterMap paramInstance, HeaderMap headerInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_GET;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_READ;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsHeader"),  this.xExternal);

			handlerInstance.Param=paramInstance;

			handlerInstance.Header=headerInstance;

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ResponseHandler>(typeof(ResponseHandler), "application/json");


		}

		/// <summary>The method to update related records</summary>
		/// <param name="recordId">long?</param>
		/// <param name="request">Instance of BodyWrapper</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> UpdateRelatedRecords(long? recordId, BodyWrapper request)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_PUT;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_UPDATE;

			handlerInstance.ContentType="application/json";

			handlerInstance.Request=request;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.UpdateRelatedRecordsHeader"),  this.xExternal);

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to delink records</summary>
		/// <param name="recordId">long?</param>
		/// <param name="paramInstance">Instance of ParameterMap</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> DelinkRecords(long? recordId, ParameterMap paramInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.CategoryMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.DelinkRecordsHeader"),  this.xExternal);

			handlerInstance.Param=paramInstance;

			Utility.GetFields( this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to get related records using external id</summary>
		/// <param name="externalValue">string</param>
		/// <param name="paramInstance">Instance of ParameterMap</param>
		/// <param name="headerInstance">Instance of HeaderMap</param>
		/// <returns>Instance of APIResponse<ResponseHandler></returns>
		public APIResponse<ResponseHandler> GetRelatedRecordsUsingExternalId(string externalValue, ParameterMap paramInstance, HeaderMap headerInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_GET;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_READ;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsUsingExternalIDHeader"),  this.xExternal);

			handlerInstance.Param=paramInstance;

			handlerInstance.Header=headerInstance;

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ResponseHandler>(typeof(ResponseHandler), "application/json");


		}

		/// <summary>The method to update related records using external id</summary>
		/// <param name="externalValue">string</param>
		/// <param name="request">Instance of BodyWrapper</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> UpdateRelatedRecordsUsingExternalId(string externalValue, BodyWrapper request)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_PUT;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_UPDATE;

			handlerInstance.ContentType="application/json";

			handlerInstance.Request=request;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.UpdateRelatedRecordsUsingExternalIDHeader"),  this.xExternal);

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to delete related records using external id</summary>
		/// <param name="externalValue">string</param>
		/// <param name="paramInstance">Instance of ParameterMap</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> DeleteRelatedRecordsUsingExternalId(string externalValue, ParameterMap paramInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.CategoryMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.DeleteRelatedRecordsUsingExternalIDHeader"),  this.xExternal);

			handlerInstance.Param=paramInstance;

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to get related record</summary>
		/// <param name="relatedRecordId">long?</param>
		/// <param name="recordId">long?</param>
		/// <param name="headerInstance">Instance of HeaderMap</param>
		/// <returns>Instance of APIResponse<ResponseHandler></returns>
		public APIResponse<ResponseHandler> GetRelatedRecord(long? relatedRecordId, long? recordId, HeaderMap headerInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, relatedRecordId.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_GET;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_READ;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordHeader"),  this.xExternal);

			handlerInstance.Header=headerInstance;

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ResponseHandler>(typeof(ResponseHandler), "application/json");


		}

		/// <summary>The method to update related record</summary>
		/// <param name="relatedRecordId">long?</param>
		/// <param name="recordId">long?</param>
		/// <param name="request">Instance of BodyWrapper</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> UpdateRelatedRecord(long? relatedRecordId, long? recordId, BodyWrapper request)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, relatedRecordId.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_PUT;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_UPDATE;

			handlerInstance.ContentType="application/json";

			handlerInstance.Request=request;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.UpdateRelatedRecordHeader"),  this.xExternal);

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to delink record</summary>
		/// <param name="relatedRecordId">long?</param>
		/// <param name="recordId">long?</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> DelinkRecord(long? relatedRecordId, long? recordId)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, recordId.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, relatedRecordId.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.CategoryMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.DelinkRecordHeader"),  this.xExternal);

			Utility.GetFields( this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to get related record using external id</summary>
		/// <param name="externalFieldValue">string</param>
		/// <param name="externalValue">string</param>
		/// <param name="headerInstance">Instance of HeaderMap</param>
		/// <returns>Instance of APIResponse<ResponseHandler></returns>
		public APIResponse<ResponseHandler> GetRelatedRecordUsingExternalId(string externalFieldValue, string externalValue, HeaderMap headerInstance)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalFieldValue.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_GET;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_READ;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordUsingExternalIDHeader"),  this.xExternal);

			handlerInstance.Header=headerInstance;

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ResponseHandler>(typeof(ResponseHandler), "application/json");


		}

		/// <summary>The method to update related record using external id</summary>
		/// <param name="externalFieldValue">string</param>
		/// <param name="externalValue">string</param>
		/// <param name="request">Instance of BodyWrapper</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> UpdateRelatedRecordUsingExternalId(string externalFieldValue, string externalValue, BodyWrapper request)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalFieldValue.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_PUT;

			handlerInstance.CategoryMethod=Constants.REQUEST_CATEGORY_UPDATE;

			handlerInstance.ContentType="application/json";

			handlerInstance.Request=request;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.UpdateRelatedRecordUsingExternalIDHeader"),  this.xExternal);

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}

		/// <summary>The method to delete related record using external id</summary>
		/// <param name="externalFieldValue">string</param>
		/// <param name="externalValue">string</param>
		/// <returns>Instance of APIResponse<ActionHandler></returns>
		public APIResponse<ActionHandler> DeleteRelatedRecordUsingExternalId(string externalFieldValue, string externalValue)
		{
			CommonAPIHandler handlerInstance=new CommonAPIHandler();

			string apiPath="";

			apiPath=string.Concat(apiPath, "/crm/v2/");

			apiPath=string.Concat(apiPath,  this.moduleAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalValue.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath,  this.relatedListAPIName.ToString());

			apiPath=string.Concat(apiPath, "/");

			apiPath=string.Concat(apiPath, externalFieldValue.ToString());

			handlerInstance.APIPath=apiPath;

			handlerInstance.HttpMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.CategoryMethod=Constants.REQUEST_METHOD_DELETE;

			handlerInstance.AddHeader(new Header<string>("X-EXTERNAL", "com.zoho.crm.api.RelatedRecords.DeleteRelatedRecordUsingExternalIDHeader"),  this.xExternal);

			Utility.GetRelatedLists( this.relatedListAPIName,  this.moduleAPIName, handlerInstance);

			return handlerInstance.APICall<ActionHandler>(typeof(ActionHandler), "application/json");


		}


		public static class GetRelatedRecordsHeader
		{
			public static readonly Header<DateTimeOffset?> IF_MODIFIED_SINCE=new Header<DateTimeOffset?>("If-Modified-Since", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsHeader");
		}


		public static class GetRelatedRecordsParam
		{
			public static readonly Param<int?> PAGE=new Param<int?>("page", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsParam");
			public static readonly Param<int?> PER_PAGE=new Param<int?>("per_page", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsParam");
		}


		public static class UpdateRelatedRecordsHeader
		{
		}


		public static class DelinkRecordsHeader
		{
		}


		public static class DelinkRecordsParam
		{
			public static readonly Param<string> IDS=new Param<string>("ids", "com.zoho.crm.api.RelatedRecords.DelinkRecordsParam");
		}


		public static class GetRelatedRecordsUsingExternalIDHeader
		{
			public static readonly Header<DateTimeOffset?> IF_MODIFIED_SINCE=new Header<DateTimeOffset?>("If-Modified-Since", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsUsingExternalIDHeader");
		}


		public static class GetRelatedRecordsUsingExternalIDParam
		{
			public static readonly Param<int?> PAGE=new Param<int?>("page", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsUsingExternalIDParam");
			public static readonly Param<int?> PER_PAGE=new Param<int?>("per_page", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordsUsingExternalIDParam");
		}


		public static class UpdateRelatedRecordsUsingExternalIDHeader
		{
		}


		public static class DeleteRelatedRecordsUsingExternalIDHeader
		{
		}


		public static class DeleteRelatedRecordsUsingExternalIDParam
		{
			public static readonly Param<string> IDS=new Param<string>("ids", "com.zoho.crm.api.RelatedRecords.DeleteRelatedRecordsUsingExternalIDParam");
		}


		public static class GetRelatedRecordHeader
		{
			public static readonly Header<DateTimeOffset?> IF_MODIFIED_SINCE=new Header<DateTimeOffset?>("If-Modified-Since", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordHeader");
		}


		public static class UpdateRelatedRecordHeader
		{
		}


		public static class DelinkRecordHeader
		{
		}


		public static class GetRelatedRecordUsingExternalIDHeader
		{
			public static readonly Header<DateTimeOffset?> IF_MODIFIED_SINCE=new Header<DateTimeOffset?>("If-Modified-Since", "com.zoho.crm.api.RelatedRecords.GetRelatedRecordUsingExternalIDHeader");
		}


		public static class UpdateRelatedRecordUsingExternalIDHeader
		{
		}


		public static class DeleteRelatedRecordUsingExternalIDHeader
		{
		}

	}
}