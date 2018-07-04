using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class LocalDataManager {

	public static readonly string DataStorageRoot = Application.persistentDataPath + "/ds";
	public static readonly string TemperDataStorageRoot = Application.persistentDataPath + "/tds";

	private static byte[] keyArray;
	private static RijndaelManaged rij;
	private static ICryptoTransform encryptor;
	private static ICryptoTransform decryptor;

	static LocalDataManager() {
		keyArray = Encoding.UTF8.GetBytes("fucknflsfucknflsfucknflsfucknfls");
		rij = new RijndaelManaged();
		rij.Key = keyArray;
		rij.Mode = CipherMode.ECB;
		rij.Padding = PaddingMode.PKCS7;
		encryptor = rij.CreateEncryptor();
		decryptor = rij.CreateDecryptor();
	}

	public static void Init() {
		if (!Directory.Exists(DataStorageRoot)) {
			Directory.CreateDirectory(DataStorageRoot);
		}

		if (!Directory.Exists(TemperDataStorageRoot)) {
			Directory.CreateDirectory(TemperDataStorageRoot);
		}
	}

	public static void MoveData(string from, string to) {
		File.Move(from, to);
	}

	public static void WriteDataToFile(string path, object data) {
		WriteDataToFile(path, JsonConvert.SerializeObject(data));
	}

	public static void WriteDataToFile(string path, string contents) {
		File.WriteAllText(path, Encrypt(contents));
	}

	public static T ReadDataFromFile<T>(string path) {
		return JsonConvert.DeserializeObject<T>(Decrypt(File.ReadAllText(path)));
	}

	public static Dictionary<string, object> ReadDataFromFile(string path) {
		return JsonConvert.DeserializeObject<Dictionary<string, object>>(Decrypt(File.ReadAllText(path)));
	}
	
	public static string Encrypt(string toEncrypt) {
		byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
		byte[] encryptedArray = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		return Convert.ToBase64String(encryptedArray, 0, encryptedArray.Length);
	}

	public static string Decrypt(string toDecrypt) {
		byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
		byte[] decryptedArray = decryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
		return Encoding.UTF8.GetString(decryptedArray);
	}
}