using UnityEngine;

/// <summary>
/// [Texture Import Setting] - [Advanced] - [Read/Write] - [On]
/// </summary>
public static class TextureExtensions
{
    public enum Watermark_Anchor { TopLeft, TopRight, BottomLeft, BottomRight }

    public static Texture2D ScaleTexure(this Texture2D texture2D, float scaleFactor)
    {
        if (scaleFactor == 1f)
        {
            return texture2D;
        }
        else if (scaleFactor == 0f)
        {
            return Texture2D.blackTexture;
        }

        int newWidth = Mathf.RoundToInt(texture2D.width * scaleFactor);
        int newHeight = Mathf.RoundToInt(texture2D.height * scaleFactor);

        Color32[] scaledTexPixels = new Color32[newWidth * newHeight];

        for (int yCord = 0; yCord < newHeight; yCord++)
        {
            float vCord = yCord / (newHeight * 1f);
            int scanLineIndex = yCord * newWidth;

            for (int xCord = 0; xCord < newWidth; xCord++)
            {
                float uCord = xCord / (newWidth * 1f);

                scaledTexPixels[scanLineIndex + xCord] = texture2D.GetPixelBilinear(uCord, vCord);
            }
        }

        Texture2D result = new Texture2D(newWidth, newHeight);
        result.SetPixels32(scaledTexPixels);
        result.Apply();

        return result;
    }

    public static Texture2D AddTexture(this Texture2D texture2D, Texture2D addTexture, int startX = -1, int startY = -1)
    {
        int addTextureWidth = addTexture.width;
        int addTextureHeight = addTexture.height;

        if (startX == -1)
        {
            startX = texture2D.width/2;
        }
        if (startY == -1)
        {
            startY = texture2D.height/2;
        }

        startX -= Mathf.CeilToInt(addTextureWidth/2);
        startY -= Mathf.CeilToInt(addTextureHeight/2);

        if (startX < 0 || startY < 0)
        {
            Debug.LogError($"Add Texture[{addTexture.name}] Start Point Out Of Range Original Texutre [{texture2D.name}]");
            return texture2D;
        }
        if((startX + addTextureWidth > texture2D.width) || (startY + addTextureHeight > texture2D.height))
        {
            Debug.LogError($"Add Texture[{addTexture.name}] End Point Out Of Range Original Texutre[{texture2D.name}]");
            return texture2D;
        }
        Color[] addPixels = addTexture.GetPixels();
        Color[] originalPixels = texture2D.GetPixels(startX, startY, addTextureWidth, addTextureHeight);

        for (int i = 0; i < addPixels.Length; i++)
        {
            Color pixel = addPixels[i];
            if (pixel.a == 0)
                continue;
            originalPixels[i] = pixel;
        }

        texture2D.SetPixels(startX, startY, addTextureWidth, addTextureHeight, originalPixels);
        texture2D.Apply();
        return texture2D;
    }

    public static Texture2D AddWaterMark(this Texture2D texture2D, Texture2D watermarkTexture, Watermark_Anchor anchor, float watermarkScaleFactor, int offSetX, int offSetY)
    {
        if (watermarkTexture == null)
        {
            texture2D.Apply();
            return texture2D;
        }
        float watermarkWidth = (watermarkTexture.width * watermarkScaleFactor);
        float watermarkHeight = (watermarkTexture.height * watermarkScaleFactor);

        offSetX = (int)(offSetX * watermarkScaleFactor);
        offSetY = (int)(offSetY * watermarkScaleFactor);

        int startY;
        int startX;
        switch (anchor)
        {
            case Watermark_Anchor.TopLeft:
                startX = offSetX + Mathf.CeilToInt(watermarkWidth/2);
                startY = texture2D.height - offSetY - Mathf.CeilToInt(watermarkHeight/2);
                break;
            case Watermark_Anchor.TopRight:
                startX = texture2D.width - offSetX -  Mathf.CeilToInt(watermarkWidth/2);
                startY = texture2D.height - offSetY - Mathf.CeilToInt(watermarkHeight/2);
                break;
            case Watermark_Anchor.BottomLeft:
                startX = offSetX + Mathf.CeilToInt(watermarkWidth/2);
                startY = offSetY + Mathf.CeilToInt(watermarkHeight/2);
                break;
            case Watermark_Anchor.BottomRight:
                startX = texture2D.width - offSetX -  Mathf.CeilToInt(watermarkWidth/2);
                startY = offSetY + Mathf.CeilToInt(watermarkHeight/2);
                break;
            default:
                startX = texture2D.width/2;
                startY = texture2D.height/2;
                break;
        }

        watermarkTexture = watermarkTexture.ScaleTexure(watermarkScaleFactor);

        return texture2D.AddTexture(watermarkTexture, startX, startY);
    }
}


