using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkRequest : IDisposable {

	public const string CONTENT_TYPE = "Content-Type";
	public const string OFFLINE_SIGN = "Cannot resolve destination host";

	protected string _url;
	protected string _dataType;
	protected string _postData;
	protected bool _disposed;
	protected Action<string> _successAction;
	protected Action<string, bool> _failAction;

	public NetworkRequest(string url, Action<string> successAction = null, Action<string, bool> failAction = null) {
		_url = url;
		_successAction = successAction;
		_failAction = failAction;
	}

	public NetworkRequest(string url, string dataType, string postData, Action<string> successAction = null, Action<string, bool> failAction = null) : this(url, successAction, failAction) {
		_dataType = dataType;
		_postData = postData;
	}

	~NetworkRequest() {
		Dispose();
	}

	public virtual IEnumerator ExeRequest() {
		WWW www = _dataType != null ? new WWW(_url, Encoding.UTF8.GetBytes(_postData), new Dictionary<string, string>{{CONTENT_TYPE, _dataType}}) : new WWW(_url);
		yield return www;
		if (www.error == null) _successAction?.Invoke(www.text);
		else _failAction?.Invoke(www.error, www.error == OFFLINE_SIGN);
	}

	public void Dispose() {
		if (_disposed) return;
		_url = null;
		_dataType = null;
		_postData = null;
		_successAction = null;
		_failAction = null;
		GC.SuppressFinalize(this);
		_disposed = true;
	}
}