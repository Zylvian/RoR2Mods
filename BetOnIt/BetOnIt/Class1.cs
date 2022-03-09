using JarlePartyPack;
using R2API;
using System.IO;

public static class SoundBank
{
	//A constant of the soundBank's name.
	public const string bankName = "Jarle_SoundBank.bnk";
	// Not necesary, but useful if you want to store the bundle on its own folder.
	public const string bankFolder= "soundBanks";

	//The direct path to your AssetBundle
	public static string SoundBankPath
	{
		get
		{
			//This returns the path to your assetbundle assuming said bundle is on the same folder as your DLL. If you have your bundle in a folder, you can uncomment the statement below this one.
			//return Path.Combine(JarlePartyPack.PInfo.Location, bankName);
			return Path.Combine(JarlePartyPackClass.PInfo.Location, bankFolder, bankName);
		}
	}

	public static void Init()
	{
		SoundAPI.SoundBanks.Add(File.ReadAllBytes(SoundBankPath));
	}
}