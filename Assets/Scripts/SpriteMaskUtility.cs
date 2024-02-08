using UnityEngine;

public static class SpriteMaskUtility
{
    public static bool IsPointInsideSpriteMask(Vector2 point, Texture2D maskTexture, SpriteMask spriteMask)
    {
        if (maskTexture == null || spriteMask == null)
        {
            Debug.LogError("Mask texture or SpriteMask component not found!");
            return false;
        }

        // Convert the world point to local space
        Vector2 localPoint = spriteMask.transform.InverseTransformPoint(point);

        // Convert local point to texture coordinates
        float textureX = (localPoint.x + spriteMask.sprite.bounds.size.x / 2) / spriteMask.sprite.bounds.size.x;
        float textureY = (localPoint.y + spriteMask.sprite.bounds.size.y / 2) / spriteMask.sprite.bounds.size.y;

        // Convert texture coordinates to pixel coordinates
        int pixelX = Mathf.FloorToInt(textureX * maskTexture.width);
        int pixelY = Mathf.FloorToInt(textureY * maskTexture.height);

        // Check if the pixel is within the mask's bounds
        if (pixelX >= 0 && pixelX < maskTexture.width && pixelY >= 0 && pixelY < maskTexture.height)
        {
            // Check if the alpha value of the pixel is above a certain threshold (considered as inside the mask)
            Color pixelColor = maskTexture.GetPixel(pixelX, pixelY);
            float alphaThreshold = 0.1f; // Adjust the threshold to consider only colored pixels
            bool isInside = pixelColor.a > alphaThreshold;

            return isInside;
        }

        // If the pixel is outside the mask's bounds, consider it outside the sprite mask
        return false;
    }
}
