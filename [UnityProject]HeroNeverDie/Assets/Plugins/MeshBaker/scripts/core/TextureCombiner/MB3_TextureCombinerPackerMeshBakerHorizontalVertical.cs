using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DigitalOpus.MB.Core
{
    internal class MB3_TextureCombinerPackerMeshBakerHorizontalVertical : MB3_TextureCombinerPackerMeshBaker
    {
        public override AtlasPackingResult[] CalculateAtlasRectangles(MB3_TextureCombinerPipeline.TexturePipelineData data, bool doMultiAtlas, MB2_LogLevel LOG_LEVEL)
        {
            Debug.Assert(!data.OnlyOneTextureInAtlasReuseTextures());

            //TODO parameter
            int maxAtlasWidth = 512;
            Debug.Assert(data._packingAlgorithm != MB2_PackingAlgorithmEnum.UnitysPackTextures, "Unity texture packer cannot be used");

            //split the list of distinctMaterialTextures into two bins
            List<MB_TexSet> horizontalVerticalDistinctMaterialTextures = new List<MB_TexSet>();
            List<MB_TexSet> regularTextures = new List<MB_TexSet>();
            for (int i = 0; i < data.distinctMaterialTextures.Count; i++)
            {
                MB_TexSet texSet = data.distinctMaterialTextures[i];
                if (texSet.idealWidth >= maxAtlasWidth &&
                    texSet.ts[0].GetEncapsulatingSamplingRect().width > 1f)
                {
                    horizontalVerticalDistinctMaterialTextures.Add(texSet);
                }
                else
                {
                    regularTextures.Add(texSet);
                }
            }
            if (LOG_LEVEL >= MB2_LogLevel.debug)
            {
                Debug.Log(String.Format("Splitting list of distinctMaterialTextures numHorizontalVertical={0} numRegular={1}", horizontalVerticalDistinctMaterialTextures.Count, regularTextures.Count));
            }

            //pack one with the horizontal texture packer
            MB2_TexturePacker tp;
            MB2_PackingAlgorithmEnum packingAlgorithm;
            int atlasMaxDimension = data._maxAtlasSize;
            AtlasPackingResult[] packerRectsHorizontalVertical;
            if (horizontalVerticalDistinctMaterialTextures.Count > 0)
            {
                packingAlgorithm = MB2_PackingAlgorithmEnum.MeshBakerTexturePacker_Horizontal;
                List<Vector2> imageSizesHorizontalVertical = new List<Vector2>();
                for (int i = 0; i < horizontalVerticalDistinctMaterialTextures.Count; i++)
                {
                    horizontalVerticalDistinctMaterialTextures[i].SetTilingTreatmentAndAdjustEncapsulatingSamplingRect(MB_TextureTilingTreatment.edgeToEdgeX);
                    imageSizesHorizontalVertical.Add(new Vector2(horizontalVerticalDistinctMaterialTextures[i].idealWidth, horizontalVerticalDistinctMaterialTextures[i].idealHeight));
                }
                tp = MB3_TextureCombinerPipeline.CreateTexturePacker(packingAlgorithm);
                tp.atlasMustBePowerOfTwo = false;
                List<AtlasPadding> paddingsHorizontalVertical = new List<AtlasPadding>();
                for (int i = 0; i < imageSizesHorizontalVertical.Count; i++)
                {
                    AtlasPadding padding = new AtlasPadding();
                    padding.topBottom = data._atlasPadding;
                    padding.leftRight = data._atlasPadding;
                    if (packingAlgorithm == MB2_PackingAlgorithmEnum.MeshBakerTexturePacker_Horizontal) padding.leftRight = 0;
                    if (packingAlgorithm == MB2_PackingAlgorithmEnum.MeshBakerTexturePacker_Vertical) padding.topBottom = 0;
                    paddingsHorizontalVertical.Add(padding);
                }
                tp.LOG_LEVEL = MB2_LogLevel.trace;
                packerRectsHorizontalVertical = tp.GetRects(imageSizesHorizontalVertical, paddingsHorizontalVertical, maxAtlasWidth, atlasMaxDimension, false);
            }
            else
            {
                packerRectsHorizontalVertical = new AtlasPackingResult[0];
            }
            AtlasPackingResult[] packerRectsRegular;
            if (regularTextures.Count > 0)
            {
                //pack other with regular texture packer
                packingAlgorithm = MB2_PackingAlgorithmEnum.MeshBakerTexturePacker;
                List<Vector2> imageSizesRegular = new List<Vector2>();
                for (int i = 0; i < regularTextures.Count; i++)
                {
                    imageSizesRegular.Add(new Vector2(regularTextures[i].idealWidth, regularTextures[i].idealHeight));
                }
                tp = MB3_TextureCombinerPipeline.CreateTexturePacker(MB2_PackingAlgorithmEnum.MeshBakerTexturePacker);
                tp.atlasMustBePowerOfTwo = false;
                List<AtlasPadding> paddingsRegular = new List<AtlasPadding>();
                for (int i = 0; i < imageSizesRegular.Count; i++)
                {
                    AtlasPadding padding = new AtlasPadding();
                    padding.topBottom = data._atlasPadding;
                    padding.leftRight = data._atlasPadding;
                    paddingsRegular.Add(padding);
                }
                tp.LOG_LEVEL = MB2_LogLevel.trace;
                packerRectsRegular = tp.GetRects(imageSizesRegular, paddingsRegular, maxAtlasWidth, atlasMaxDimension, false);
            }
            else
            {
                packerRectsRegular = new AtlasPackingResult[0];
            }

            AtlasPackingResult result = null;
            if (packerRectsHorizontalVertical.Length == 0 && packerRectsRegular.Length == 0)
            {
                Debug.Assert(false, "Should never have reached this.");
                return null;
            }
            else if (packerRectsHorizontalVertical.Length > 0 && packerRectsRegular.Length > 0)
            {
                result = MergeAtlasPackingResultStackBonA(packerRectsHorizontalVertical[0], packerRectsRegular[0], atlasMaxDimension, maxAtlasWidth, true);
            }
            else if (packerRectsHorizontalVertical.Length > 0)
            {
                Debug.LogError("TODO handle this");
                result = packerRectsHorizontalVertical[0];
            }
            else if (packerRectsRegular.Length > 0)
            {
                Debug.LogError("TODO handle this.");
                result = packerRectsRegular[0];
            }

            Debug.Assert(data.distinctMaterialTextures.Count == result.rects.Length);
            //We re-ordered the distinctMaterial textures so replace the list with the new reordered one
            horizontalVerticalDistinctMaterialTextures.AddRange(regularTextures);
            data.distinctMaterialTextures = horizontalVerticalDistinctMaterialTextures;
            AtlasPackingResult[] results;
            if (result != null) results = new AtlasPackingResult[] { result };
            else results = new AtlasPackingResult[0]; 
            return results;
        }

        public static AtlasPackingResult MergeAtlasPackingResultStackBonA(AtlasPackingResult a,
            AtlasPackingResult b, int maxHeightDim, int maxWidthDim, bool stretchBToAtlasWidth)
        {
            Debug.Assert(a.usedW == a.atlasX);
            Debug.Assert(a.usedH == a.atlasY);
            Debug.Assert(b.usedW == b.atlasX);
            Debug.Assert(b.usedH == b.atlasY);
            Debug.Assert(a.usedW <= maxWidthDim, a.usedW + " " + maxWidthDim);
            Debug.Assert(a.usedH <= maxHeightDim, a.usedH + " " + maxHeightDim);
            Debug.Assert(b.usedH <= maxHeightDim);
            Debug.Assert(b.usedW <= maxWidthDim);

            Rect AatlasToFinal;
            Rect BatlasToFinal;

            // first calc height scale and offset
            float finalH = a.usedH + b.usedH;
            float scaleYa, scaleYb;
            if (finalH > maxHeightDim)
            {
                scaleYa = maxHeightDim / finalH; //0,1
                float offsetBy = ((float)Mathf.FloorToInt(a.usedH * scaleYa)) / maxHeightDim;//0,1
                scaleYa = offsetBy;
                scaleYb = (1f - offsetBy);
                AatlasToFinal = new Rect(0, 0, 1, scaleYa);
                BatlasToFinal = new Rect(0, offsetBy, 1, scaleYb);
            }
            else
            {
                float offsetBy = a.usedH / finalH;
                AatlasToFinal = new Rect(0, 0, 1, offsetBy);
                BatlasToFinal = new Rect(0, offsetBy, 1, b.usedH / finalH);
            }

            //next calc width scale and offset
            if (a.atlasX > b.atlasX)
            {
                if (!stretchBToAtlasWidth)
                {
                    // b rects will be placed in a larger atlas which will make them smaller
                    BatlasToFinal.width = ((float)b.atlasX) / a.atlasX;
                }
            }
            else if (b.atlasX > a.atlasX)
            {
                // a rects will be placed in a larger atlas which will make them smaller
                AatlasToFinal.width = ((float)a.atlasX) / b.atlasX;
            }

            Rect[] newRects = new Rect[a.rects.Length + b.rects.Length];
            AtlasPadding[] paddings = new AtlasPadding[a.rects.Length + b.rects.Length];
            int[] srcImgIdxs = new int[a.rects.Length + b.rects.Length];
            Array.Copy(a.padding, paddings, a.padding.Length);
            Array.Copy(b.padding, 0, paddings, a.padding.Length, b.padding.Length);
            Array.Copy(a.srcImgIdxs, srcImgIdxs, a.srcImgIdxs.Length);
            Array.Copy(b.srcImgIdxs, 0, srcImgIdxs, a.srcImgIdxs.Length, b.srcImgIdxs.Length);
            Array.Copy(a.rects, newRects, a.rects.Length);
            int atlasX = Mathf.Max(a.usedW, b.usedW);
            int atlasY = a.usedH + b.usedH;
            float maxx = 0f;
            float maxy = 0f;
            for (int i = 0; i < a.rects.Length; i++)
            {
                Rect r = a.rects[i];
                r.x = AatlasToFinal.x + r.x * AatlasToFinal.width;
                r.y = AatlasToFinal.y + r.y * AatlasToFinal.height;
                r.width *= AatlasToFinal.width;
                r.height *= AatlasToFinal.height;
                maxx = Mathf.Max(maxx, r.xMax);
                maxy = Mathf.Max(maxy, r.yMax);
                newRects[i] = r;
                srcImgIdxs[i] = a.srcImgIdxs[i];
            }

            for (int i = 0; i < b.rects.Length; i++)
            {
                Rect r = b.rects[i];
                r.x = BatlasToFinal.x + r.x * BatlasToFinal.width;
                r.y = BatlasToFinal.y + r.y * BatlasToFinal.height;
                r.width *= BatlasToFinal.width;
                r.height *= BatlasToFinal.height;
                maxx = Mathf.Max(maxx, r.xMax);
                maxy = Mathf.Max(maxy, r.yMax);
                newRects[a.rects.Length + i] = r;
                srcImgIdxs[a.rects.Length + i] = b.srcImgIdxs[i];
            }

            AtlasPackingResult res = new AtlasPackingResult(paddings);

            res.atlasX = atlasX;
            res.atlasY = atlasY;
            res.padding = paddings;
            res.rects = newRects;
            res.srcImgIdxs = srcImgIdxs;
            res.CalcUsedWidthAndHeight();

            return res;
        }
    }
}
