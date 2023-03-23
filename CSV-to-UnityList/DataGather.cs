//Created by Angshuman 'Moz' Mazumdar

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a Class Item to store data headers from the dataset. Please adjust the variables accordingly, to match the column headers of your dataset file.
/// The dataset originally being used here was the Chicago crime dataset, thus the current variables are corresponding to the column headers of that dataset.
/// </summary>
[System.Serializable]
public class DataElementContainer
{
    public int grid_x_loc;
    public int grid_y_loc;
    public string caseNo;
    public string dateOfOccurrence;
    public string blockAddress;
    public int IUCR;
    public string primaryDescription;
    public string secondaryDescription;
    public string locationType;
    public string arrestMade;
    public string domestic;
    public int beat;
    public int ward;
    public string fbi_cd;
    public long x_coord;
    public long y_coord;
    public float latitude;
    public float longitude;
}

public class DataGather : MonoBehaviour
{
    /// <summary>
    /// Create a singleton of this Script - basically this can be accessed from anywhere (any other script)
    /// </summary>
    public static DataGather DataGatherInstance;

    [Header("Required")]
    [SerializeField] TextAsset datasetFile;

    [SerializeField] string[] datasetLines;
    [SerializeField] DataElementContainer[] datasetElements;

    //List<DataElement> oldList;

    //Total number of lines in the TXT file (or CSV)
    public int lineCount = 0;

    /// <summary>
    /// Set up the encapsulated properties, and make it so that the values cannot be written from external scripts (edited from outside) but any information can be read from external scripts.
    /// </summary>
    public TextAsset DatasetFile { get => datasetFile; private set => datasetFile = value; }
    public string[] DatasetLines { get => datasetLines; private set => datasetLines = value; }
    public DataElementContainer[] DatasetElements { get => datasetElements; private set => datasetElements = value; }

    private void Awake()
    {
        DataGatherInstance = this;
    }

    void OnValidate()
    {
        //From the loaded txt file, extract using the NEWLINE delimiter and extract each row as a string and set it into Array #1 (array of string) e.g. {"1,0,0", "0,1,0"}
        DatasetLines = DatasetFile ? DatasetFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;
        lineCount = DatasetLines.Length;
        DatasetElements = new DataElementContainer[DatasetLines.Length];

        //For each entry in Array 1
        for (int i = 0; i < lineCount; i++)
        {
            //Further split it according to comma delimiter, and set each of those values int Array #2 (array of string) e.g {"1","0","0"}, {"0", "1", "0"}
            var datasetContentPart = DatasetLines[i].Split(',');
            //Debug.Log(datasetContentPart.Length);

            //VERY basic validation query. TODO: CHANGE IT SO ITS NOT HARD CODED. 18 value comes from number of properties in the DataElement class.
            if (datasetContentPart.Length % 18 != 0)
            {
                Debug.LogError("Atleast 18 comma separated values needed!"); //TODO - make the 18 dependent on the number of data element container variables
                return;
            }

            //Set the fields for each element of Array #3 (array of DataElementContainer class)
            DatasetElements[i] = SetDataElementValues(datasetContentPart);
        }

        //oldList = DatasetElements.ToList();

    }

    /// <summary>
    /// Function to take each line got from separating the lines of the txt file using NEWLINE and comma delimiter, and populate each Class Item's properties.
    /// </summary>
    /// <param name="datasetLine"> Each entry from the second string array (after splitting using comma delimiter).</param>
    /// <returns></returns>
    DataElementContainer SetDataElementValues(string[] datasetLine)
    {
        return new DataElementContainer
        {
            grid_x_loc = int.TryParse(datasetLine[0], out var xl) ? xl : 0,
            grid_y_loc = int.TryParse(datasetLine[1], out var yl) ? yl : 0,
            caseNo = datasetLine[2],
            dateOfOccurrence = datasetLine[3],
            blockAddress = datasetLine[4],
            IUCR = int.TryParse(datasetLine[5], out var i) ? i : 0,
            primaryDescription = datasetLine[6],
            secondaryDescription = datasetLine[7],
            locationType = datasetLine[8],
            arrestMade = datasetLine[9],
            domestic = datasetLine[10],
            beat = int.TryParse(datasetLine[11], out var b) ? b : 0,
            ward = int.TryParse(datasetLine[12], out var w) ? w : 0,
            fbi_cd = datasetLine[13],
            x_coord = int.TryParse(datasetLine[14], out var x) ? x : 0,
            y_coord = int.TryParse(datasetLine[15], out var y) ? y : 0,
            latitude = float.TryParse(datasetLine[16], out var la) ? la : 0,
            longitude = float.TryParse(datasetLine[17], out var lo) ? lo : 0
        };
    }
}
