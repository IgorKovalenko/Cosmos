﻿using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Kernel;
using Cosmos.Hardware;
using Indy.IL2CPU.Plugs;

namespace Cosmos.Kernel.Plugs {
	[Plug(Target = typeof(System.Console))]
	class Console {
        private static ConsoleColor _background = ConsoleColor.Black;
        private static ConsoleColor _foreground = ConsoleColor.White;

        public static ConsoleColor get_ForegroundColor()
        {
            return _foreground;
        }

        public static void set_ForegroundColor(ConsoleColor value)
        {
            _foreground = value;
            TextScreen.SetColors(_foreground, _background);
        }

        public static ConsoleColor get_BackgroundColor()
        {
            return _background;
        }

        public static void set_BackgroundColor(ConsoleColor value)
        {
            _background = value;
            TextScreen.SetColors(_foreground, _background);
        }

        public static int get_CursorLeft()
        {
            return TextScreen.CurrentChar;
        }

		public static int get_CursorTop() {
            return TextScreen.CurrentRow;
		}

        public static void set_CursorLeft(int x)
        {
            TextScreen.CurrentChar = x;
        }

		public static void set_CursorTop(int y) {
            TextScreen.CurrentRow = y;
		}

		public static int get_WindowHeight() {
            return TextScreen.Rows;
		}

		public static int get_WindowWidth() {
			return TextScreen.Columns;
		}
					   
		//TODO: Console uses TextWriter - intercept and plug it instead
		public static void Clear() {
            TextScreen.Clear();
            set_CursorLeft(0);
            set_CursorTop(0);
		}

		public static void Write(char aChar) {
			TextScreen.WriteChar(aChar);
		}

		public static void Write(string aText) {
			for (int i = 0; i < aText.Length; i++) {
				if (aText[i] == '\n') {
					TextScreen.NewLine();
					continue;
				}
				if (aText[i] == '\r') {
					continue;
				}
				TextScreen.WriteChar(aText[i]);
			}
		}

		public static void WriteLine(string aLine) {
			Write(aLine);
			TextScreen.NewLine();
		}

		public static void WriteLine(uint aValue) {
			WriteLine(aValue.ToString());
		}

		public static void WriteLine(int aValue) {
			WriteLine(aValue.ToString());
		}

		public static void Write(char[] buffer) {
			for (int i = 0; i < buffer.Length; i++)
				Write(buffer[i]);
		}

		public static void WriteLine(char[] buffer) {
			Write(buffer);
			WriteLine();
		}

		public static void WriteLine() {
			TextScreen.NewLine();
		}

		public static string ReadLine() {
			List<char> chars = new List<char>(32);
			char current;
			// HACK: convert this to "while ((current = Keyboard.ReadChar()) != '\n') {" 
			//   MTW: SOmehow an invalid opcode exception is occurring.
			while (true) {
			    current = Cosmos.Hardware.Keyboard.ReadChar();
				if (current == '\n') {
					break;
				} else if (current == '\u0968') { // Backspace   
                    if (chars.Count > 0) {
                        chars.RemoveAt(chars.Count - 1);
                        TextScreen.CurrentChar--;
                        TextScreen.WriteChar(' ');
                        TextScreen.CurrentChar--;
                    }
                    continue;
                }
				chars.Add(current);
				Write(current);
			}
			WriteLine();

			// HACK: Should use .ToArray here.
			char[] final = new char[chars.Count];
            for (int i = 0; i < final.Length; i++) {
                final[i] = chars[i];
            }

			return new string(final);
		}
	}
}
