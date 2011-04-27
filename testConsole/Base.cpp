#include "StdAfx.h"
#include "Base.h"

#include <stdio.h>

using namespace std;

#pragma warning (disable:4482)

CFruitBase::CFruitBase(void)
{
}

CFruitBase::~CFruitBase(void)
{
}

EFruitType CFruitBase::CallPrivateMethod()
{
	return MyOverridablePrivateMethod();
}

/*virtual*/ EFruitType CFruitBase::MyOverridablePublicMethod()
{
	cout << "CBase::MyOverridablePublicMethod()" << endl;

	return EFruitType::BASE;
}

//private:
/*virtual*/ EFruitType CFruitBase::MyOverridablePrivateMethod()
{
	cout << "CBase::MyOverridablePrivateMethod()" << endl;

	return EFruitType::BASE;
}
