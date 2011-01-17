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
 * File:    DisplayObject.h
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once

#include "Point.h"
#include <iostream>
#include <fstream>

using namespace std;

class DisplayObject
{
protected:
    Point _centre;
    int _colour;
    void setCentre(GWindow&);
    void printPoint(GWindow&, Point, const char*);
    int parse(string);

public:
    virtual void show(GWindow&);
    virtual void prompt(GWindow&);
    virtual void save(fstream&);
    virtual void load(GWindow&, vector<string>&);
    virtual void perform(GWindow&);
    int getColour();
    void setColour(int);
    Point getCentre();
    void setCentre(Point);
    DisplayObject(void);
    ~DisplayObject(void);
};
