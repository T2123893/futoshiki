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
 * File:    DisplayObject.cpp
 * Date:    12/02/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */


#include "stdafx.h"
#include "DisplayObject.h"

void DisplayObject::perform(GWindow &gw) {}

void DisplayObject::prompt(GWindow &gw) {}

void DisplayObject::show(GWindow &gw) {}

//void DisplayObject::save(char *fileName) {}
void DisplayObject::save(fstream &out) {}

void DisplayObject::load(GWindow &gw, vector<string> &attr) {}

//void DisplayObject::erase(GWindow &gw) 
//{
//    gw.setPenColour(GColour(228,228,228));
//    gw.rectangle(199,54,600,200);
//    gw.setPenColour(BLACK);
//}

void DisplayObject::printPoint(GWindow &gw, Point p, const char *label)
{
    gw.writeText(label);
    gw.writeText("(x=");
    gw.writeInt(p.getX());
    gw.writeText(", y=");
    gw.writeInt(p.getY());
    gw.writeText(")");
}

int DisplayObject::getColour()
{
    return _colour;
}

void DisplayObject::setColour(int colour)
{
    _colour = colour;
}

Point DisplayObject::getCentre()
{
    return _centre;
}

void DisplayObject::setCentre(Point centre)
{
    _centre = centre;
}

void DisplayObject::setCentre(GWindow &gw)
{
    int x, y;
    gw.writeText(200, 55, "Centre Point X: ");
    x = gw.readInt(290, 55);
    gw.writeText(200, 75, "Centre Point Y: ");
    y = gw.readInt(290, 75);
    this->_centre = Point(x,y);
}

int DisplayObject::parse(string s)
{
    return atoi(const_cast<const char *>(s.c_str()));
}

DisplayObject::DisplayObject(void)
{
}

DisplayObject::~DisplayObject(void)
{
    cout << "基类对象销毁了" << endl;
}
