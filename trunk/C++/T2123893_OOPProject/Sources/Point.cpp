/**
 * $Id$
 * 
 * SOFT40051 - Object-oriented Programming in C++ Assignment
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 *
 * File:    Point.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#include "stdafx.h"
#include "Point.h"

void Point::setY(int y)
{
    _y = y;
}

int Point::getY()
{
    return _y;
}

void Point::setX(int x)
{
    _x = x;
}

int Point::getX()
{
    return _x;
}

Point::Point(int x, int y)
{
    _x = x;
    _y = y;
}

Point::Point(void)
{
    _x = 0;
    _y = 0;
}

Point::~Point(void)
{
}
