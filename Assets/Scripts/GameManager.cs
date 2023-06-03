using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BattleShip
{
    public class GameManager : MonoBehaviour
    {       
        [SerializeField] string trainingName;
        [SerializeField] int archiveLimit;
        [SerializeField] public bool record;

        List<int> records;
        // Start is called before the first frame update
        void Start()
        {
            records = new List<int>(); 
        }

        public void Record(int gameLength)
        {
            records.Add(gameLength);

            if(records.Count == archiveLimit)
            {
                string outputFile = "Logs/Results/" + this.trainingName + ".txt";
                string turns = "";
                int sum = 0;
                foreach(int count in records)
                {
                    sum += count;
                }
                turns += (sum/archiveLimit);
                StreamWriter results;

                if (!File.Exists(outputFile))
                {
                    results = new StreamWriter(outputFile);
                }
                else
                {
                    results = File.AppendText(outputFile);
                }

                results.WriteLine(turns);
                // Close the stream:
                results.Close();
                records.Clear();
            }
          
        }
    
    }
}
