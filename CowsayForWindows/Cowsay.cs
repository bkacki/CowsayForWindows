﻿using System;
using System.IO;
using System.Collections.Generic;

namespace CowsayForWindows
{
    public static class Cowsay
    {
        private static readonly string _fileName = "cow.csv";
        private static readonly string _appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _filePath = Path.Combine(_appDirectory, "models", _fileName);
        private static readonly int _blockSize = 40;

        public static void SayMessage(string message)
        {
            List<string> messageWithBorder = _addBorderToMessage(message);
            foreach (string line in messageWithBorder)
                Console.WriteLine(line);
            _showCow();
        }

        private static List<string> _addBorderToMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message can't be empty or whitespace.");

            var messageWithBorder = new List<string>();
            if (message.Length < _blockSize)
            {
                string upperBorder = " " + new string('_', message.Length + 2);
                string bottomBorder = " " + new string('-', message.Length + 2);
                string text = $"< {message} >";

                messageWithBorder.Add(upperBorder);
                messageWithBorder.Add(text);
                messageWithBorder.Add(bottomBorder);
            }
            else
            {
                var blocks = _splitStringIntoBlocks(message);
                string upperBorder = " " + new string('_', _blockSize + 2);
                string bottomBorder = " " + new string('-', _blockSize + 2);

                messageWithBorder.Add(upperBorder);
                messageWithBorder.Add($"/ {blocks[0]} \\");

                for (int i = 1; i < blocks.Count - 1; i++)
                {
                    messageWithBorder.Add($"| {blocks[i]} |");
                }

                messageWithBorder.Add($"\\ {blocks[^1]} /");
                messageWithBorder.Add(bottomBorder);
            }
            return messageWithBorder;
        }

        private static List<string> _splitStringIntoBlocks(string str)
        {
            var blocks = new List<string>();
            var words = str.Split(' ');
            var currentBlock = "";

            foreach (string word in words)
            {
                if ((currentBlock + word).Length <= _blockSize)
                {
                    currentBlock += (currentBlock == "" ? "" : " ") + word;
                }
                else
                {
                    blocks.Add(currentBlock.PadRight(_blockSize));
                    currentBlock = word;
                }
            }

            if (!string.IsNullOrEmpty(currentBlock))
                blocks.Add(currentBlock.PadRight(_blockSize));

            return blocks;
        }

        private static void _showCow()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine($"File {_fileName} was not found in the application folder.");
                    return;
                }

                var csvLines = File.ReadAllLines(_filePath);
                foreach (var line in csvLines)
                    Console.WriteLine(line);
            }
            catch (IOException ex)
            {
                Console.WriteLine("An I/O error occurred while trying to read the file.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access to the file is denied.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
        }
    }
}