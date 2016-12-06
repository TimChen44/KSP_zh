using System;
using System.IO;

internal class ReferenceProvider
{
    internal static byte[] UncheckMemory(long childValues, Stream currentManager)
    {
        byte num8 = (byte)currentManager.ReadByte();
        byte num = (byte)num8;
        num = (byte)~num;
        for (int i = 1; i < 3; i++)
        {
            currentManager.ReadByte();
        }
        byte[] buffer = new byte[currentManager.Length - currentManager.Position];
        currentManager.Read(buffer, 0, buffer.Length);
        if ((num & 0x20) != 0)
        {
            for (int j = 0; j < buffer.Length; j++)
            {
                buffer[j] = (byte)~buffer[j];
            }
        }
        return buffer;
    }
}
