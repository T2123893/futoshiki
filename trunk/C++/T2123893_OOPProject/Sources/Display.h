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
 * File:    Display.h
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once


#include <list>
#include <vector>
#include <iostream>
#include <fstream>
#include "DisplayObject.h"
#include "Triangle.h"
#include "Circle.h"
#include "Rectangl.h"

using namespace std;

class Display
{
private:
    enum { CIRCLE=99, RECTANGLE=114, TRIANGLE=116};
    DisplayObject *_object;
    list<DisplayObject*> _objList;
    list<DisplayObject*>::iterator _it;
    void checkEnteredID(GWindow&);
    void split(string, string, vector<string>&);

public:
    void quit(GWindow&);
    void zap(GWindow&);
    void load(GWindow&);
    void save(GWindow&);
    void erase(GWindow&);
    void del(GWindow&);
    void edit(GWindow&);
    void show(GWindow&, int top=55);
    void add(GWindow&, int);
    void perform(GWindow&);
    void performAll();
    Display(void);
    ~Display(void);
};
