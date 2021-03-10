using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Helpers
{
    public class ReindexRequiredException : Exception
    {
        public ReindexRequiredException()
        {
        }

        public ReindexRequiredException(string message)
            : base(message)
        {
        }

        public ReindexRequiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public static class Positioning
    {
        private const string _alphabet = "abcdefghijklmnopqrstuvwxyz";
        public const string EndStop = "zzzzzzzzzzzzzzzzzzzzz";

        public static string Calculate(string previous, string after)
        {
            if(previous == null)
            {
                previous = "a";
            }

            if(after == null)
            {
                after = EndStop;
            }

            int i = 0;
            string position = "";

            if(previous.Length == 0 || after.Length == 0)
            {
                throw new IndexOutOfRangeException("One position has length 0");
            }

            while(true)
            {
                if(previous.Length <= i && after.Length <= i)
                {
                    // Full reindex
                } else if(previous.Length <= i)
                {
                    int posAft = _alphabet.IndexOf(after[i]);

                    if(posAft == 0) // prev is "aa" aft is "aaa"
                    {
                        // Full reindex
                        throw new ReindexRequiredException();
                    } else
                    {
                        position += _alphabet[(int)Math.Floor((0 + posAft) / 2.0)];
                        break;
                    }
                } else if(after.Length <= i)
                {
                    int posPrev = _alphabet.IndexOf(previous[i]);

                    if (posPrev == 25)
                    {
                        // Full reindex
                        throw new ReindexRequiredException();
                    }
                    else
                    {
                        position += _alphabet[(int)Math.Ceiling((posPrev + 25) / 2.0)];
                        break;
                    }
                } else if(!previous[i].Equals(after[i]))
                {
                    int posPrev = _alphabet.IndexOf(previous[i]);
                    int posAft = _alphabet.IndexOf(after[i]);

                    if(Math.Abs(posPrev - posAft) == 1)
                    {
                        // Increase length
                        position += previous[i];
                        if(previous.Length > i + 1)
                        {
                            position += Calculate(previous.Substring(i + 1), null);
                        }
                        else // prev ends with this char
                        {
                            position += _alphabet[(int)Math.Floor((0 + 25) / 2.0)];
                        }
                        break;
                    } else
                    {
                        position += _alphabet[(int)Math.Floor((posPrev + posAft) / 2.0)];
                        break;
                    }
                } else
                {
                    position += previous[i];
                }

                i++;
            }

            return position;
        }

        public static string Max(string pos1, string pos2)
        {
            int comp = String.Compare(pos1, pos2);

            if(comp >= 1)
            {
                return pos1;
            } else
            {
                return pos2;
            }
        }

        public static List<string> GetDistribution(int taskCount, string prev = null, string next = null)
        {
            prev = prev ?? "a";
            next = next ?? EndStop;

            List<string> Positions = new List<string>();

            if(taskCount <= 0)
            {
                return Positions;
            } else if(taskCount == 1)
            {
                Positions.Add(Calculate(prev, next));
            } else
            {
                string pos = Calculate(prev, next);
                Positions.AddRange(GetDistribution((int)Math.Floor((taskCount-1) / 2.0), prev, pos));
                Positions.Add(pos);
                Positions.AddRange(GetDistribution((int)Math.Ceiling((taskCount - 1) / 2.0), pos, next));
            }

            return Positions;
        }
    }
}
