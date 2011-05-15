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
 * File:    Rectangle.h
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once

#include "Point.h"
#include "DisplayObject.h"

class Rectangl : public DisplayObject
{
private:
    Point _a;
    Point _b;
    Point _c;
    Point _d;

public:
    void perform(GWindow&);
    void load(GWindow&, vector<string>&);
    void save (fstream&);
    void prompt(GWindow&);
    void show(GWindow&);
    void setA(Point);
    Point getA(void);
    //void setB(Point);
    Point getB(void);
    //void setC(void);
    Point getC(void);
    //void setD(void);
    Point getD(void);
    Rectangl(Point, Point);
    Rectangl(void);
    ~Rectangl(void);
};
