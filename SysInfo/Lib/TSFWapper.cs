﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using TSF;

public class TSFWapper
{
    public static short[] GetLangIDs()
    {
        List<short> langIDs = new List<short>();
        ITfInputProcessorProfiles profiles;
        if (TSF_NativeAPI.TF_CreateInputProcessorProfiles(out profiles) == 0)
        {
            IntPtr langPtrs;
            int fetchCount = 0;
            if (profiles.GetLanguageList(out langPtrs, out fetchCount) == 0)
            {
                for (int i = 0; i < fetchCount; i++)
                {
                    short id = Marshal.ReadInt16(langPtrs, sizeof(short) * i);
                    langIDs.Add(id);
                }
            }
            Marshal.ReleaseComObject(profiles);
        }
        return langIDs.ToArray();
    }

    public static string[] GetInputMethodList(short langID)
    {
        List<string> imeList = new List<string>();
        ITfInputProcessorProfiles profiles;
        if (TSF_NativeAPI.TF_CreateInputProcessorProfiles(out profiles) == 0)
        {
            try
            {
                IEnumTfLanguageProfiles enumerator = null;
                if (profiles.EnumLanguageProfiles(langID, out enumerator) == 0)
                {
                    if (enumerator != null)
                    {
                        TF_LANGUAGEPROFILE[] langProfile = new TF_LANGUAGEPROFILE[1];
                        int fetchCount = 0;
                        while (enumerator.Next(1, langProfile, out fetchCount) == 0)
                        {
                            IntPtr ptr;
                            if (profiles.GetLanguageProfileDescription(ref langProfile[0].clsid,
                                langProfile[0].langid, ref langProfile[0].guidProfile, out ptr) == 0)
                            {
                                bool enabled;
                                if (profiles.IsEnabledLanguageProfile(ref langProfile[0].clsid,
                                    langProfile[0].langid, ref langProfile[0].guidProfile, out enabled) == 0)
                                {
                                    if (enabled)
                                        imeList.Add(Marshal.PtrToStringBSTR(ptr));
                                }
                            }
                            Marshal.FreeBSTR(ptr);
                        }
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(profiles);
            }
        }
        return imeList.ToArray();
    }

    ///
    /// 获取当前输入法
    ///
    public static void GetCurrentLang(out string[] lang)
    {
        List<short> langIDs = new List<short>();
        ITfInputProcessorProfiles profiles;
        short id = -1;
        if (TSF_NativeAPI.TF_CreateInputProcessorProfiles(out profiles) == 0)
        {
            //IntPtr langPtrs;
            //int fetchCount = 0;
            profiles.GetCurrentLanguage(out id);
            Marshal.ReleaseComObject(profiles);
        }
        lang = id > -1 ? GetInputMethodList(id) : null;
    }
}