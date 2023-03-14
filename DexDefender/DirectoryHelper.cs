
using System;
using System.IO;
using System.Threading.Tasks;

public class DirectoryHelper
{
    // 基本删除方法
    public static bool DeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"删除失败: {ex.Message}");
            return false;
        }
    }

    // 安全删除方法（带重试机制）
    public static bool SafeDeleteDirectory(string path, int retryCount = 3)
    {
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }
                return false;
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(500);
            }
            catch (UnauthorizedAccessException)
            {
                System.Threading.Thread.Sleep(500);
            }
        }
        return false;
    }

    // 异步删除方法
    public static async Task<bool> DeleteDirectoryAsync(string path)
    {
        return await Task.Run(() => DeleteDirectory(path));
    }
}
