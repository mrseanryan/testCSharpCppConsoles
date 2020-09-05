#pragma once
#include "base.h"

class CFruit :
	public CFruitBase
{
public:
	CFruit(void);
	virtual ~CFruit(void);

	//this class has no implementation of MyOverridablePublicMethod()
};
