using System;
using System.IO;
using MTools;
using UnityEngine;

public class CVSReader
{
    private int CELLS_BYTES_OFFSET;

    private int KEY_BYTES_OFFSET;

    private int MAX_ROWS = -1;

    private int MAX_COLUMNS = -1;

    private CSVTypeInfo[] typeLookUp;

    private bool isFileOpen;

    private StreamReader reader;

    public CVSReader(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("filePath is null or empty.");
            return;
        }
        Open(filePath);
    }

    public CVSReader(Stream stream)
    {
        if (stream == null)
        {
            Debug.LogError("stream is null.");
            return;
        }
        Open(stream);
    }

    public CVSReader()
    {
    }

    public int GetRowCount()
    {
        return MAX_ROWS;
    }

    public int GetColCount()
    {
        return MAX_COLUMNS;
    }

    public int GetCellBytesOffset()
    {
        return CELLS_BYTES_OFFSET;
    }

    public int GetKeyBytesOffset()
    {
        return KEY_BYTES_OFFSET;
    }

    public CSVTypeInfo[] GetTypeInfoTable()
    {
        return typeLookUp;
    }

    ~CVSReader()
    {
        Close();
        reader = null;
    }

    public void Close()
    {
        if (!isFileOpen)
        {
            return;
        }
        try
        {
            if (reader != null)
            {
                reader.Close();
            }
            reader = null;
            isFileOpen = false;
            Debug.Log("File closed.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error while closing file. " + ex.Message);
        }
    }

    public bool IsOpen()
    {
        return isFileOpen;
    }

    public bool Open(string path)
    {
        if (isFileOpen)
        {
            Debug.LogWarning("A file is already open. Close it before opening another.");
            return false;
        }

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Path is null or empty.");
            return false;
        }

        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found at original path: " + path);

            string fileName = Path.GetFileName(path);

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("Could not extract filename from path: " + path);
                return false;
            }

            string streamingPath = Path.Combine(Application.streamingAssetsPath, "export/" + fileName);
            Debug.Log("Trying StreamingAssets fallback: " + streamingPath);

            if (File.Exists(streamingPath))
            {
                Debug.Log("Found in StreamingAssets -> " + streamingPath);
                path = streamingPath;
            }
            else
            {
                Debug.LogError("File not found in StreamingAssets either: " + streamingPath);
                return false;
            }
        }
        else
        {
            Debug.Log("Opening file at original path -> " + path);
        }

        try
        {
            reader = new StreamReader(path);
            isFileOpen = true;
            Debug.Log("File opened successfully -> " + path);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception while opening file: " + ex.Message);
            isFileOpen = false;
        }

        if (isFileOpen)
        {
            ParseTypeLine();
        }

        return isFileOpen;
    }

    public bool Open(Stream stream)
    {
        if (isFileOpen)
        {
            Debug.LogWarning("A file is already open. Close it before opening another.");
            return false;
        }

        if (stream == null)
        {
            Debug.LogError("Invalid File Stream — stream is null.");
            return false;
        }

        try
        {
            reader = new StreamReader(stream);
            isFileOpen = true;
            Debug.Log("Stream opened successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception while opening stream: " + ex.Message);
            isFileOpen = false;
        }

        if (isFileOpen)
        {
            ParseTypeLine();
        }

        return isFileOpen;
    }

    public string ReadLine()
    {
        if (reader == null)
        {
            Debug.LogError("Cannot ReadLine — reader is null.");
            return null;
        }
        return reader.ReadLine();
    }

    private bool IsSkipLine(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }

        char[] array = str.ToCharArray();
        for (int i = 0; i < str.Length; i++)
        {
            if (array[i] != ',' && array[i] != '_')
            {
                return false;
            }
        }
        return true;
    }

    public MArray ParseLine(ref string key)
    {
        string text = ReadLine();

        while (text != null && IsSkipLine(text))
        {
            text = ReadLine();
        }

        if (text == null)
        {
            Debug.LogWarning("ParseLine reached end of file — no more lines.");
            return null;
        }

        if (typeLookUp == null)
        {
            Debug.LogError("typeLookUp is null. ParseTypeLine may not have run correctly.");
            return null;
        }

        char[] array = text.ToCharArray();
        int num = 0;

        if (array.Length > 0 && array[num] == ',')
        {
            num++;
            while (num < text.Length && array[num] == ',')
            {
                if (array[num] == ',')
                {
                    num++;
                }
            }
            if (num > text.Length)
            {
                Debug.LogWarning("ParseLine — line only had commas, skipping.");
                return null;
            }
        }

        MArray mArray = new MArray(array.Length);
        MObject obj = null;
        int num2 = 0;
        string text2 = null;
        sbyte b = -1;

        for (int i = num; i <= array.Length; i++)
        {
            if (i != array.Length && array[i] == '"')
            {
                b = (sbyte)((b != -1) ? 1 : 0);
            }

            if (i != array.Length && (array[i] != ',' || b == 0))
            {
                continue;
            }

            b = -1;
            text2 = new string(array, num, i - num);
            text2 = TrimString(text2);

            if (key == null)
            {
                key = text2;
                KEY_BYTES_OFFSET += 2;
                KEY_BYTES_OFFSET += 1 * text2.Length;
            }

            if (num2 >= typeLookUp.Length)
            {
                Debug.LogWarning("Column index " + num2 + " exceeds typeLookUp length " + typeLookUp.Length + ". Skipping.");
                break;
            }

            try
            {
                switch (typeLookUp[num2].id)
                {
                    case TypeID.TYPE_INT:
                        if (string.IsNullOrEmpty(text2))
                        {
                            Debug.LogWarning("Empty int value at column " + num2 + ", defaulting to 0.");
                            obj = new MObject(0);
                        }
                        else
                        {
                            obj = new MObject(int.Parse(text2));
                        }
                        CELLS_BYTES_OFFSET += 4;
                        break;

                    case TypeID.TYPE_FLOAT:
                        if (string.IsNullOrEmpty(text2))
                        {
                            Debug.LogWarning("Empty float value at column " + num2 + ", defaulting to 0f.");
                            obj = new MObject(0f);
                        }
                        else
                        {
                            obj = new MObject(float.Parse(text2));
                        }
                        CELLS_BYTES_OFFSET += 4;
                        break;

                    case TypeID.TYPE_STRING:
                        int length = text2.Length;
                        if (length >= 3 && text2[0] == '"')
                        {
                            text2 = text2.Substring(1, length - 2);
                        }
                        obj = new MObject(text2 ?? string.Empty);
                        CELLS_BYTES_OFFSET += 2;
                        CELLS_BYTES_OFFSET += 1 * length;
                        break;

                    default:
                        Debug.LogWarning("Unknown type at column " + num2 + " for value: " + text2);
                        obj = new MObject(text2 ?? string.Empty);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to convert value \"" + text2 + "\" at column " + num2 + ". " + ex.Message);
                throw ex;
            }

            mArray.addObject(obj);
            num2++;
            num = i;
            num++;
        }

        return mArray;
    }

    private void ParseTypeLine()
    {
        string text = ReadLine();

        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("ParseTypeLine — first line is null or empty. File may be corrupt or empty.");
            return;
        }

        char[] array = text.ToCharArray();
        int num = 0;
        int num2 = 0;
        int num3 = 0;

        if (array.Length != 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == ',')
                {
                    num3++;
                }
            }
            MAX_COLUMNS = num3 + 1;
        }

        if (MAX_COLUMNS <= 0)
        {
            Debug.LogError("ParseTypeLine — MAX_COLUMNS is " + MAX_COLUMNS + ". Type line may be malformed.");
            return;
        }

        Debug.Log("Parsed type line — " + MAX_COLUMNS + " columns found.");

        typeLookUp = new CSVTypeInfo[MAX_COLUMNS];
        int num4 = 0;

        for (num2 = num; num2 <= array.Length; num2++)
        {
            if (num2 == array.Length || array[num2] == ',')
            {
                string data = new string(array, num, num2 - num);

                if (string.IsNullOrEmpty(data))
                {
                    Debug.LogWarning("ParseTypeLine — empty column definition at index " + num4 + ".");
                }
                else
                {
                    SetTypeData(data, num4);
                }

                num4++;
                num = num2;
                num++;
            }
        }

        Debug.Log("Type lookup table built successfully.");
    }

    private void SetTypeData(string data, int colNum)
    {
        if (string.IsNullOrEmpty(data))
        {
            Debug.LogWarning("SetTypeData — data is null or empty at column " + colNum + ".");
            typeLookUp[colNum].id = TypeID.TYPE_UNKNOWN;
            return;
        }

        char[] value = data.ToCharArray();
        int index = 0;
        int num = 0;

        if (data[index] == 'i')
        {
            typeLookUp[colNum].id = TypeID.TYPE_INT;
        }
        else if (data[index] == 'f')
        {
            typeLookUp[colNum].id = TypeID.TYPE_FLOAT;
        }
        else if (data[index] == 's')
        {
            typeLookUp[colNum].id = TypeID.TYPE_STRING;
        }
        else
        {
            Debug.LogWarning("SetTypeData — unknown type char '" + data[index] + "' at column " + colNum + ". Defaulting to TYPE_UNKNOWN.");
            typeLookUp[colNum].id = TypeID.TYPE_UNKNOWN;
        }

        index = 2;
        num = data.Length;

        if (index >= num)
        {
            Debug.LogWarning("SetTypeData — column name missing at column " + colNum + ".");
            typeLookUp[colNum].colName = string.Empty;
        }
        else
        {
            typeLookUp[colNum].colName = new string(value, index, num - index);
        }
    }

    private static string TrimString(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        str = str.Trim();

        while (str.Length > 0)
        {
            char c = str[0];
            if (c == '\0' || c >= ' ')
            {
                break;
            }
            str = str.Remove(0, 1);
        }

        while (str.Length > 0)
        {
            char c2 = str[str.Length - 1];
            if (c2 == '\0' || c2 >= ' ')
            {
                break;
            }
            str = str.Remove(str.Length - 1);
        }

        return str;
    }
}
