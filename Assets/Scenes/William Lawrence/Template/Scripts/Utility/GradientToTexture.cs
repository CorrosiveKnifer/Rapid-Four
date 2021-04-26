using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using System.IO;

[ExecuteAlways]
public class GradientToTexture : MonoBehaviour
{
#if UNITY_EDITOR
    public string m_ExportPath;
    public string m_ExportName;

    public int m_TextureResolution;

    public Gradient m_TextureGradient;

    public bool m_Export;

    private void Update()
    {
        if (m_Export)
        {
            Export();
            m_Export = false;
        }
    }

    private void Export()
    {
        Texture2D tex = new Texture2D(m_TextureResolution, m_TextureResolution, TextureFormat.ARGB32, true);

        Color[] colours = new Color[m_TextureResolution * m_TextureResolution];
        for (int i = 0; i < m_TextureResolution; i++)
        {
            Color col = m_TextureGradient.Evaluate((float)i / (float)(m_TextureResolution - 1));
            for (int w = 0; w < m_TextureResolution; w++)
            {
                colours[(i) * m_TextureResolution + w] = col;
            }
        }
        tex.SetPixels(colours);
        tex.Apply();

        byte[] texBytes = tex.EncodeToPNG();
        DestroyImmediate(tex);

        File.WriteAllBytes(m_ExportPath + m_ExportName + ".png", texBytes);
        System.GC.Collect();

        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}