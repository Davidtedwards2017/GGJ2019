using System.Collections;
using System.Collections.Generic;
using System;

namespace _DataPersistance
{
    public abstract class StatSerializer
    {
        public Stat CreateInstance(string statName)
        {
            var type = GetStatType(statName);
            if (type == null) return null;

            return (Stat)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Takes in stat name and returns the stat type associated with it
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public abstract Type GetStatType(string statName);
    }
}