#include "rbx/Debug.h"
#include "rbx/MathUtil.h"
#include <float.h>
#include <cmath>

namespace RBX{

int GetMappedDOF(int dof)
{
	if (dof <= 0)
	{
		return 0;
	}
	else if (dof <= 30)
	{
		return dof;
	}
	else if (dof < 40)
	{
		return 31;
	}
	else if (dof < 50)
	{
		return 32;
	}
	else if (dof < 60)
	{
		return 33;
	}
	else if (dof < 80)
	{
		return 34;
	}
	else if (dof < 100)
	{
		return 35;
	}
	else if (dof < 120)
	{
		return 36;
	}
	else if (dof == 120) 
	{
		return 37;
	}
	return 38;
};

const double DOFs[][5] = {
	{ 1,6.314,12.71,63.66,636.6 },
	{ 2,2.92,4.303,9.925,31.6 },
	{ 3,2.353,3.182,5.841,12.92 },
	{ 4,2.132,2.776,4.604,8.61 },
	{ 5,2.015,2.571,4.032,6.869 },
	{ 6,1.943,2.447,3.707,5.959 },
	{ 7,1.895,2.365,3.499,5.408 },
	{ 8,1.86,2.306,3.355,5.041 },
	{ 9,1.833,2.262,3.25,4.781 },
	{ 10,1.812,2.228,3.169,4.587 },
	{ 11,1.796,2.201,3.106,4.437 },
	{ 12,1.782,2.179,3.055,4.318 },
	{ 13,1.771,2.16,3.012,4.221 },
	{ 14,1.761,2.145,2.977,4.14 },
	{ 15,1.753,2.131,2.947,4.073 },
	{ 16,1.746,2.12,2.921,4.015 },
	{ 17,1.74,2.11,2.898,3.965 },
	{ 18,1.734,2.101,2.878,3.922 },
	{ 19,1.729,2.093,2.861,3.883 },
	{ 20,1.725,2.086,2.845,3.85 },
	{ 21,1.721,2.08,2.831,3.819 },
	{ 22,1.717,2.074,2.819,3.792 },
	{ 23,1.714,2.069,2.807,3.767 },
	{ 24,1.711,2.064,2.797,3.745 },
	{ 25,1.708,2.06,2.787,3.725 },
	{ 26,1.706,2.056,2.779,3.707 },
	{ 27,1.703,2.052,2.771,3.69 },
	{ 28,1.701,2.048,2.763,3.674 },
	{ 29,1.699,2.045,2.756,3.659 },
	{ 30,1.697,2.042,2.75,3.646 },
	{ 40,1.684,2.021,2.704,3.551 },
	{ 50,1.676,2.009,2.678,3.496 },
	{ 60,1.671,2,2.66,3.46 },
	{ 80,1.664,1.99,2.639,3.416 },
	{ 100,1.66,1.984,2.626,3.39 },
	{ 120,1.658,1.98,2.617,3.373 },
	{ 121,1.645,1.96,2.576,3.291 },
};


inline double TCritical(unsigned dof, Confidence conf)
{
	if (dof == 0)
		return DBL_MAX;

	int mappedDOF = GetMappedDOF(dof);

	RBXASSERT(conf < 4);
	RBXASSERT(mappedDOF < ARRAYSIZE(DOFs));

	return DOFs[mappedDOF][conf+1];
}


double IsValueOutlier(double value, unsigned count, double average, double std, Confidence conf)
{
	double tCritical = TCritical(count - 1, conf);
	double tScore = fabs((value - average) / std);
	return tScore > tCritical;
}

void GetConfidenceInterval(double average, double variance, Confidence conf, double* minV, double* maxV)
{
	double std = sqrt(variance);

	switch (conf)
	{
	case C90:
		if (minV)
			*minV = average - std * 1.644;

		if (maxV)
			*maxV = average + std * 1.644;
		break;
	case C95:
		if (minV)
			*minV = average - std * 2;

		if (maxV)
			*maxV = average + std * 2;
		break;

	case C99:
		if (minV)
			*minV = average - std * 3;

		if (maxV)
			*maxV = average + std * 3;
		break;

	case C99p9:
		if (minV)
			*minV = average - std * 4;

		if (maxV)
			*maxV = average + std * 4;
		break;

	default:
		return;
	}
}

}
