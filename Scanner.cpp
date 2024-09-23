#include <iostream>
#include <fstream>
#include <string>
#include <queue>
#include "Scanner.h"
#include "Lexical.h"

using namespace std;

void scanLine(string line, queue<Lexical> *mainQueue, int lineNumber) {
	Lexical lex;
	bool identifier = false;
	for (int x = 0; x < line.size(); x++) {
		//Iterates though each character in a given line
		switch (line[x]) {
			case ')':
			case '=':
			case '+':
			case '-':
			case '{':
			case '}':
			case ',':
			case '*':
			case ';':
			case '(':
			case '[':
			case ']':
			case '!':
				if (identifier) {
					//If its not a special character
					lex.endColumn = x - 1;
					lex.setLexical(line.substr(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
					mainQueue->push(lex);
					identifier = false;
				}
				lex.startColumn = x;
				lex.endColumn = x;
				lex.line = lineNumber;
				lex.setLexical(string(1, line[x]));
				mainQueue->push(lex);
				lex = Lexical();
				break;

			case '>':
			case '<':
				lex.startColumn = x;
				lex.line = lineNumber;
				if (line[x + 1] == '=') {
					lex.endColumn = x + 1;
					string temp = string(1, line[x]);
					temp += "=";
					lex.setLexical(temp);
					x++;
					//Determines if it is an equal symbol
				}
				else {
					lex.endColumn = x;
					lex.setLexical(string(1, line[x]));
				}
				mainQueue->push(lex);
				lex = Lexical();
				break;

			case '/':
				if (line[x + 1] == '/') {
					//Runs if its a comment
					lex.startColumn = x;
					lex.endColumn = (line.length() - 1);
					lex.line = lineNumber;
					mainQueue->push(lex);
					return;
				}
				//Runs if not a comment
				if (identifier) {
					lex.endColumn = x - 1;
					lex.setLexical(line.substr(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
					mainQueue->push(lex);
					identifier = false;
				}
				lex.startColumn = x;
				lex.endColumn = x;
				lex.line = lineNumber;
				lex.setLexical(string(1, line[x]));
				mainQueue->push(lex);
				lex = Lexical();
				break;
				
			case '"':
				lex.startColumn = x;
				lex.line = lineNumber;
				do {
					x++;
				} while (line[x] != '"');
				//Iterates till anothre " character is found
				lex.endColumn = x;
				lex.setLexical(line.substr(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
				mainQueue->push(lex);
				lex = Lexical();
				//Adds everthing inbetween the quotes as a single lexical
				break;

			case ' ':
			case '\t':
				if (identifier) {
					lex.endColumn = x - 1;
					lex.setLexical(line.substr(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
					mainQueue->push(lex);
					identifier = false;
				}
				lex = Lexical();
				break;

			default:
				if (!identifier) {
					//if this is the first character in identifier runs
					identifier = true;
					lex.startColumn = x;
					lex.line = lineNumber;
				}
				break;
		}
	}
	if (identifier) {
		//If line ends and a multiCharacter is still active runs
		lex.endColumn = line.size() - 1;
		lex.setLexical(line.substr(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
		mainQueue->push(lex);
	}
}

queue<Lexical> scan(string file) {
	queue<Lexical> queue;
	ifstream myfile(file);
	//Opens file given from argument
	string line;
	if (myfile.is_open())
	{
		int lineNumber = 1;
		while (getline(myfile, line))
		{
			scanLine(line, &queue, lineNumber);
			lineNumber++;
			//Iterates though each line one by one and calls the scanLine function with each line
		}
		myfile.close();
	}
	else {
		cout << "Error: Unable to Open File";
	}
	return queue;
}