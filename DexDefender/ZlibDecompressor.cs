
using System;
using System.IO;
using System.IO.Compression;

public static class ZlibDecompressor
{
    public static byte[] Inflate(byte[] compressedData)
    {
        using (var ms = new MemoryStream(compressedData))
        using (var outStream = new MemoryStream())
        {
            // 跳过Zlib头部2字节
            ms.Position = 2;

            using (var decompressor =
                new DeflateStream(ms, CompressionMode.Decompress))
            {
                decompressor.CopyTo(outStream);
            }
            return outStream.ToArray();
        }
    }

    public static byte[] Deflate(byte[] rawData)
    {
        using (var ms = new MemoryStream())
        {
            // 添加Zlib头部 (0x78 0x9C)
            ms.Write(new byte[] { 0x78, 0x9C }, 0, 2);

            using (var compressor =
                new DeflateStream(ms, CompressionLevel.Optimal, true))
            {
                compressor.Write(rawData, 0, rawData.Length);
            }
            return ms.ToArray();
        }
    }

    public static void InflateFile(string inputPath, string outputPath)
    {
        using (FileStream fsIn = File.OpenRead(inputPath))
        using (FileStream fsOut = File.Create(outputPath))
        {
            // 跳过Zlib头部2字节 (0x78 0x9C)
            fsIn.Position = 2;

            using (var decompressor = new DeflateStream(fsIn, CompressionMode.Decompress))
            {
                decompressor.CopyTo(fsOut);
            }
        }
    }


    public static void DeflateFile(string inputPath, string outputPath)
    {
        try
        {
            using (FileStream fsIn = File.OpenRead(inputPath))
            using (FileStream fsOut = File.Create(outputPath))
            {
                // 添加Zlib头部 (0x78 0x9C)
                fsOut.Write(new byte[] { 0x78, 0x9C }, 0, 2);

                using (var compressor = new DeflateStream(fsOut, CompressionLevel.Optimal))
                {
                    fsIn.CopyTo(compressor);
                }
            }
        }
        catch(Exception e)
        {
            return;
        }
       
    }

}
