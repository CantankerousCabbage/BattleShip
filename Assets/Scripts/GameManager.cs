using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BattleShip
{
    public class GameManager : MonoBehaviour
    {     
        //Name that results are submitted to. Used for generating graphs  
        [SerializeField] string trainingName;
        //Average games result is calculated off before written to file
        [SerializeField] int archiveLimit;
        //Enables records
        [SerializeField] public bool record;
        List<int> records;
        
        void Start()
        {
            records = new List<int>(); 
        }
        //Function for writing results to file
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
