/*
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
 * File:    Point.h
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once


/**
 * This class is describe a point.
 */
class Point
{
private:
    int _x;
    int _y;

public:
    void setX(int);
    int getX();
    void setY(int);
    int getY();
    Point(int, int);
    Point(void);
    ~Point(void);
};
