#include <iostream>
#include <fstream>
#include <string>
#include <queue>
#include "Scanner.h"
#include "Lexical.h"

int main(int argc, char* argv[]) {
	std::queue <Lexical> queue = scan(argv[1]);
	//calls the scan function with with the file name given as an argument
	int size = queue.size();
	for (int x = 0; x < size; x++)
	{
		Lexical lex = queue.front();
		queue.pop();
		std::cout << lex.lexical << "\tline " << lex.line << " Cols ";
		std::cout << lex.startColumn + 1 << " - " << lex.endColumn + 1;
		std::cout << " is " << lex.token << "\n";
		//Iterates though all the lexicals and prints out the information
	}
}