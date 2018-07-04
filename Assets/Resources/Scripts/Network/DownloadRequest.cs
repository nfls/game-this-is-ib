using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class DownloadRequest : NetworkRequest {

	protected string _path;
	protected string _temperPath;

	public DownloadRequest(string url, string path, string temperPath, Action<string> successAction = null, Action<string, bool> failAction = null) : base(url, successAction, failAction) {
		_path = path;
		_temperPath = path;
	}

	public override IEnumerator ExeRequest() {
		WWW www = new WWW(_url);
		yield return www;
		if (www.error == null) {
			File.WriteAllBytes(_temperPath, www.bytes);
			File.Move(_temperPath, _path);
			if (_successAction != null) {
				_successAction(null);
			}
		} else {
			if (_failAction != null) {
				_failAction(www.error, www.error == OFFLINE_SIGN);
			}
		}
	}
}