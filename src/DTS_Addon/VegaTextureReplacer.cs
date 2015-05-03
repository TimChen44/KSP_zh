//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine;

//[KSPAddon(KSPAddon.Startup.Instantly, true)]
//public class VegaTextureReplacer : MonoBehaviour
//{
//    [CompilerGenerated]
//    private static Predicate<GameDatabase.TextureInfo> PPkno;
//    private static readonly string DIR_PREFIX = "TextureReplacer/Textures/";
//    private bool isInitialised;
//    private bool isReplaceScheduled;
//    private bool isTextureCompressorDetected;
//    private int lastCrewCount;
//    private int lastMaterialsCount;
//    private int lastTextureCount;
//    private Vessel lastVessel;
//    private Dictionary<string, Texture2D> mappedTextures = new Dictionary<string, Texture2D>();
//    private int memorySpared;
//    private int updateCounter;

//    private void compressTextures()
//    {
//        List<GameDatabase.TextureInfo> databaseTexture = GameDatabase.Instance.databaseTexture;
//        if (this.lastTextureCount != databaseTexture.Count)
//        {
//            for (int i = this.lastTextureCount; i < databaseTexture.Count; i++)
//            {
//                Texture2D texture = databaseTexture[i].texture;
//                UnityEngine.TextureFormat format = texture.format;
//                switch (format)
//                {
//                    case UnityEngine.TextureFormat.DXT1:
//                    case UnityEngine.TextureFormat.DXT5:
//                        break;

//                    default:
//                        {
//                            try
//                            {
//                                texture.filterMode = FilterMode.Trilinear;
//                                texture.GetPixel(0, 0);
//                                texture.Compress(true);
//                            }
//                            catch (UnityException)
//                            {
//                                break;
//                            }
//                            int num2 = texture.width * texture.height;
//                            int num3 = (texture.format != UnityEngine.TextureFormat.Alpha8) ? ((texture.format != UnityEngine.TextureFormat.RGB24) ? (num2 * 4) : (num2 * 3)) : num2;
//                            int num4 = (texture.format != UnityEngine.TextureFormat.DXT1) ? num2 : (num2 / 2);
//                            int num5 = num3 - num4;
//                            this.memorySpared += num5;
//                            object[] args = new object[] { texture.name, format, texture.width, texture.height, ((double)num5) / 1024.0 };
//                            MonoBehaviour.print(string.Format("[TextureReplacer] Compressed {0} [{1} {2}x{3}], spared {4:0.0} KiB", args));
//                            break;
//                        }
//                }
//            }
//            this.lastTextureCount = databaseTexture.Count;
//        }
//    }


//    private void initialiseReplacer()
//    {
//        if (PPkno == null)
//        {//定义查找条件
//            PPkno = texInfo => texInfo.name.StartsWith(DIR_PREFIX);
//        }
//        //对所有贴图进行替换
//        foreach (GameDatabase.TextureInfo info in GameDatabase.Instance.databaseTexture.FindAll(PPkno))
//        {
//            if (info.texture != null)
//            {
//                string key = info.texture.name.Substring(DIR_PREFIX.Length);
//                if (this.mappedTextures.ContainsKey(key))
//                {
//                    MonoBehaviour.print("[TextureReplacer] Corrupted GameDatabase! Problematic TGA? " + info.texture.name);
//                }
//                else
//                {
//                    MonoBehaviour.print("[TextureReplacer] Mapping " + key + " -> " + info.texture.name);
//                    this.mappedTextures.Add(key, info.texture);
//                }
//            }
//        }
//        this.replaceTextures((Material[])Resources.FindObjectsOfTypeAll(typeof(Material)));
//    }

//    private void replaceTextures(Material[] materials)
//    {
//        MonoBehaviour.print("[TextureReplacer] Replacing textures and setting trilinear filter ...");
//        foreach (Material material in materials)
//        {
//            Texture mainTexture = material.mainTexture;
//            if (((mainTexture != null) && (mainTexture.name.Length != 0)) && !mainTexture.name.StartsWith("Temp"))
//            {
//                if (!this.mappedTextures.ContainsKey(mainTexture.name))
//                {
//                    if (mainTexture.filterMode == FilterMode.Bilinear)
//                    {
//                        mainTexture.filterMode = FilterMode.Trilinear;
//                    }
//                }
//                else
//                {
//                    Texture2D textured = this.mappedTextures[mainTexture.name];
//                    if (textured != mainTexture)
//                    {
//                        material.mainTexture = textured;
//                        Resources.UnloadAsset(mainTexture);
//                        MonoBehaviour.print("[TextureReplacer] " + mainTexture.name + " replaced");
//                        Texture texture = material.GetTexture("_BumpMap");
//                        if ((texture != null) && this.mappedTextures.ContainsKey(texture.name))
//                        {
//                            Texture2D textured2 = this.mappedTextures[texture.name];
//                            if (textured2 != texture)
//                            {
//                                material.SetTexture("_BumpMap", textured2);
//                                Resources.UnloadAsset(texture);
//                                MonoBehaviour.print("[TextureReplacer] " + texture.name + " [normal map] replaced");
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }

//    protected void Start()
//    {
//        UnityEngine.Object.DontDestroyOnLoad(this);
//        foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
//        {
//            if (obj2.name == "TextureCompressor")
//            {
//                MonoBehaviour.print("[TextureReplacer] Detected TextureCompressor, disabling texture compression");
//                this.isTextureCompressorDetected = true;
//                break;
//            }
//        }
//    }

//    protected void Update()
//    {
//        if (!this.isInitialised)
//        {
//            if (!this.isTextureCompressorDetected)
//            {
//                this.compressTextures();
//            }
//            if (GameDatabase.Instance.IsReady())
//            {
//                MonoBehaviour.print(string.Format("[TextureReplacer] Texture compression spared total {0:0.0} MiB = {1:0.0} MB", (((double)this.memorySpared) / 1024.0) / 1024.0, (((double)this.memorySpared) / 1000.0) / 1000.0));
//                this.initialiseReplacer();
//                this.isInitialised = true;
//            }
//        }
//        else if (HighLogic.LoadedSceneIsFlight)
//        {
//            this.lastMaterialsCount = 0;
//            this.updateCounter = 0;
//            if ((FlightGlobals.ActiveVessel != this.lastVessel) || (this.lastVessel.GetCrewCount() != this.lastCrewCount))
//            {
//                this.lastVessel = FlightGlobals.ActiveVessel;
//                this.lastCrewCount = this.lastVessel.GetCrewCount();
//                this.isReplaceScheduled = true;
//            }
//            else if (this.isReplaceScheduled)
//            {
//                this.isReplaceScheduled = false;
//                this.replaceTextures((Material[])Resources.FindObjectsOfTypeAll(typeof(Material)));
//            }
//        }
//        else if (HighLogic.LoadedScene == GameScenes.MAINMENU)
//        {
//            this.lastVessel = null;
//            this.lastCrewCount = 0;
//            this.isReplaceScheduled = false;
//            this.updateCounter--;
//            if (this.updateCounter <= 0)
//            {
//                this.updateCounter = 10;
//                Material[] materials = (Material[])Resources.FindObjectsOfTypeAll(typeof(Material));
//                if (materials.Length != this.lastMaterialsCount)
//                {
//                    this.lastMaterialsCount = materials.Length;
//                    this.replaceTextures(materials);
//                }
//            }
//        }
//        else
//        {
//            this.lastMaterialsCount = 0;
//            this.updateCounter = 0;
//            this.lastVessel = null;
//            this.lastCrewCount = 0;
//            this.isReplaceScheduled = false;
//        }
//    }
//}

