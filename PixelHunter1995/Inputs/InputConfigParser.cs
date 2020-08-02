using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.Inputs
{
    using KeyDisjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;
    using KeyConjunction = HashSet<Dictionary<Either<Keys, MouseKeys>, SignalState>>;
    
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
        
        private static Dictionary<string, Dictionary<Action, KeyConjunction>> ParseKeyConfig(string path) {
            
            var contexts = new Dictionary<string, Dictionary<Action, KeyConjunction>>();
            
            string filename = Path.GetFileName(path);
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
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
                        if (contexts.TryGetValue(context, out var binds))
                        {
                            binds = new Dictionary<Action, KeyConjunction>();
                            contexts[context] = binds;
                        }
                        binds[action] = ParseConjunction(kv[1]);
                    }
                }
            }
            return contexts;
        }
        
        private static KeyConjunction ParseConjunction(string conjunction)
        {
            KeyConjunction keyConjunction = new KeyConjunction();
            foreach (string term in conjunction.Split('+'))
            {
                KeyDisjunction keyDisjunction = new KeyDisjunction();
                foreach (string factor in term.Split('|'))
                {
                    string key = factor.Trim();
                    SignalState state = SignalState.Down;
                    if (key.Contains("^"))
                    {
                        state = SignalState.Up;
                        key.Replace("^", "");
                    }
                    if (key.Contains("~"))
                    {
                        state.IsEdge = true;
                        key.Replace("~", "");
                    }
                    
                    if (Keys.TryParse(key, true, out Keys keyboardKey))
                    {
                        keyDisjunction[keyboardKey] = state;
                    }
                    else if (MouseKeys.TryParse(key, true, out MouseKeys mouseKey))
                    {
                        keyDisjunction[mouseKey] = state;
                    }
                    else
                    {
                        Console.Error.WriteLine(String.Format("ERROR! - Unable to parse key: {0}", key));
                    }
                }
                keyConjunction.Add(keyDisjunction);
            }
            return keyConjunction;
        }
        
    }
}