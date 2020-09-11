#pragma once

namespace MyCommand
{
	class Command
	{
		public:
			static const int Header = 123;
		int num;
		int error;
		int size;
		int data;
			static const int End = 321;

		Command();
		Command(const int &num, const int &data);

		int Get_Num();
		int Get_Data();
		int Get_Size();

	};

}