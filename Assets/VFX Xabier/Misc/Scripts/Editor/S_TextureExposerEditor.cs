using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Compression;

public class S_TextureExporterEditor : Editor
{
    // A�ade una opci�n al men� contextual al hacer clic derecho sobre una o m�s texturas
    [MenuItem("Assets/Exportar Textura(s) como PNG", true)]
    private static bool ValidateExportTexturesAsPNG()
    {
        // Valida si todas las selecciones son texturas
        foreach (var obj in Selection.objects)
        {
            if (!(obj is Texture2D))
                return false;
        }
        return true;
    }

    [MenuItem("Assets/Exportar Textura(s) como PNG")]
    private static void ExportTexturesAsPNG()
    {
        // Si solo hay una textura seleccionada, exportarla como PNG
        if (Selection.objects.Length == 1)
        {
            ExportSingleTexture(Selection.activeObject as Texture2D);
        }
        // Si hay m�s de una textura seleccionada, exportarlas todas a un ZIP
        else if (Selection.objects.Length > 1)
        {
            ExportTexturesAsZIP();
        }
    }

    // Exporta una �nica textura como PNG
    private static void ExportSingleTexture(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("No se ha seleccionado ninguna textura.");
            return;
        }

        // Pide al usuario que elija la ubicaci�n para guardar el archivo
        string path = EditorUtility.SaveFilePanel("Guardar textura como PNG", "", texture.name + ".png", "png");

        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("Se ha cancelado la exportaci�n.");
            return;
        }

        // Convierte la textura a formato PNG
        byte[] pngData = texture.EncodeToPNG();

        if (pngData != null)
        {
            // Guarda el archivo PNG en la ruta especificada
            File.WriteAllBytes(path, pngData);
            Debug.Log("Textura exportada en: " + path);
        }
        else
        {
            Debug.LogError("No se pudo convertir la textura a PNG.");
        }
    }

    // Exporta m�ltiples texturas como PNG dentro de un archivo ZIP
    private static void ExportTexturesAsZIP()
    {
        // Pide al usuario que elija la ubicaci�n para guardar el archivo ZIP
        string zipPath = EditorUtility.SaveFilePanel("Guardar texturas como ZIP", "", "texturas.zip", "zip");

        if (string.IsNullOrEmpty(zipPath))
        {
            Debug.Log("Se ha cancelado la exportaci�n.");
            return;
        }

        // Crea un directorio temporal para guardar los PNGs
        string tempFolderPath = Path.Combine(Application.temporaryCachePath, "TexturesTemp");
        Directory.CreateDirectory(tempFolderPath);

        // Recorre todas las texturas seleccionadas y las exporta como PNG en la carpeta temporal
        foreach (var selectedObject in Selection.objects)
        {
            Texture2D texture = selectedObject as Texture2D;

            if (texture != null)
            {
                string texturePath = Path.Combine(tempFolderPath, texture.name + ".png");

                // Convierte la textura a formato PNG
                byte[] pngData = texture.EncodeToPNG();

                if (pngData != null)
                {
                    // Guarda el archivo PNG en la carpeta temporal
                    File.WriteAllBytes(texturePath, pngData);
                }
                else
                {
                    Debug.LogError("No se pudo convertir la textura a PNG: " + texture.name);
                }
            }
        }

        // Crea el archivo ZIP que contendr� todas las texturas PNG
        ZipFile.CreateFromDirectory(tempFolderPath, zipPath);

        // Elimina el directorio temporal
        Directory.Delete(tempFolderPath, true);

        Debug.Log("Texturas exportadas y comprimidas en: " + zipPath);
    }
}
