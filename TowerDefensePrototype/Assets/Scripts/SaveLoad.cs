using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ZombieDefense
{

        [Serializable]
        public class ScoreTable
        {
            public List<Score> Scores = new List<Score>();

            public ScoreTable()
            {
                // for serializer
            }

            public ScoreTable([NotNull] List<Score> scores)
            {
                Scores = scores ?? throw new ArgumentNullException(nameof(scores));
            }
        }

        [Serializable]
        public class Score
        {
            public string Name;
            public int Points;

            public Score()
            {
                // for serializer
            }

            public Score(string name, int points)
            {
                Name = name;
                Points = points;
            }
        }

        public class SaveLoad
        {
            public static void SaveScore(string name, int points)
            {
                ScoreTable data = LoadScore();

                Score newScore = new Score(name, points);

                data.Scores.Add(newScore);

                string json = JsonUtility.ToJson(data);

                File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
            }

            public static ScoreTable LoadScore()
            {
                string path = Application.persistentDataPath + "/savefile.json";
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    ScoreTable data = JsonUtility.FromJson<ScoreTable>(json);
                    return data;
                }

                return null;
            }



        public static IEnumerable<Score> GetHighestScores()
        {
            ScoreTable scores = LoadScore();
            if (scores != null) return scores.Scores.OrderByDescending(s => s.Points).Take(10);
            else return null;
        }

        }
    }

