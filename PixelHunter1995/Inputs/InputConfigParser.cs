using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.Inputs
{
    using KeyDisjunction = List<Dictionary<Either<Keys, MouseKeys>, SignalState>>;
    using KeyConjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;
    
    class InputConfigParser
    {
        
        public static readonly string DEFAULT_CONTEXT = "DEFAULT";
        
        public static Dictionary<string, Input> ParseInputConfig(string path)
        {
            var inputs = new Dictionary<string, Input>();
            foreach (var kv in ParseKeyConfig(path)) {
                var context = kv.Key;
                var binds = kv.Value;
                inputs[context] = new Input(binds);
            }
            return inputs;
        }
        
        private static Dictionary<string, Dictionary<Action, KeyDisjunction>> ParseKeyConfig(string path) {
            
            var contexts = new Dictionary<string, Dictionary<Action, KeyDisjunction>>();
            
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string context;
                    string actionStr;
                    
                    string[] kv = line.Split('=');
                    string lhs = kv[0].Trim();
                    
                    // Make it possible to load keybinds for specific "contexts"
                    // Could potentially be used for ie. secondary players,
                    // or states.
                    if (lhs.Contains("."))
                    {
                        string[] pair = lhs.Split('.');
                        context = pair[0];
                        actionStr = pair[1];
                    }
                    else
                    {
                        context = DEFAULT_CONTEXT;
                        actionStr = lhs;
                    }
                    
                    if (Action.TryParse(kv[0].Trim(), true, out Action action))
                    {
                        if (!contexts.TryGetValue(context, out var binds))
                        {
                            binds = new Dictionary<Action, KeyDisjunction>();
                            contexts[context] = binds;
                        }
                        binds[action] = ParseConjunction(kv[1]);
                    }
                }
            } else {
                Console.Error.WriteLine(String.Format("ERROR! - Unable to find config file! {0}", path));
            }
            return contexts;
        }
        
        private static KeyDisjunction ParseConjunction(string disjunction)
        {
            KeyDisjunction keyDisjunction = new KeyDisjunction();
            foreach (string conjunction in disjunction.Split('|'))
            {
                KeyConjunction keyConjunction = new KeyConjunction();
                foreach (string term in conjunction.Split('+'))
                {
                    string key = term.Trim();
                    bool isUp = false;
                    bool isEdge = false;
                    if (key.Contains("^"))
                    {
                        isUp = true;
                        key = key.Replace("^", "");
                    }
                    if (key.Contains("~"))
                    {
                        isEdge = true;
                        key = key.Replace("~", "");
                    }
                    SignalState state = new SignalState(isUp, isEdge);
                    if (Keys.TryParse(key, true, out Keys keyboardKey))
                    {
                        keyConjunction[keyboardKey] = state;
                    }
                    else if (MouseKeys.TryParse(key, true, out MouseKeys mouseKey))
                    {
                        keyConjunction[mouseKey] = state;
                    }
                    else
                    {
                        Console.Error.WriteLine(String.Format("ERROR! - Unable to parse key: {0}", key));
                    }
                }
                keyDisjunction.Add(keyConjunction);
            }
            return keyDisjunction;
        }
        
    }
}