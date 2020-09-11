#include "Command.h"

namespace MyCommand
{
	Command::Command() : num(0), data(0)
	{
	}

	Command::Command(const int &num, const int &data)
	{
		this->num = num;
		this->data = data;
	}

	int Command::Get_Num()
	{
		return num;
	}

	int Command::Get_Data()
	{
		return data;
	}

	int Command::Get_Size()
	{
		return sizeof(data);
	}

}