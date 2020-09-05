#include "StdAfx.h"
#include "Orange.h"

#include <stdio.h>

using namespace std;

#pragma warning (disable:4482)

COrange::COrange(void)
{
}

COrange::~COrange(void)
{
}

/*virtual*/ EFruitType COrange::MyOverridablePublicMethod()
{
	cout << "COrange::MyOverridablePublicMethod()" << endl;

	return EFruitType::ORANGE;
}

//private:
/*virtual*/ EFruitType COrange::MyOverridablePrivateMethod()
{
	cout << "COrange::MyOverridablePrivateMethod()" << endl;

	return EFruitType::ORANGE;
}