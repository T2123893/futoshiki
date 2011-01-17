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
 * File:    Circle.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */


#include "stdafx.h"
#include "Circle.h"


void Circle::perform(GWindow &gw)
{
    gw.outlineCircle(this->_centre.getX(), this->_centre.getY(), _diameter/2);
}

void Circle::load(GWindow &gw, vector<string> &attr)
{
    this->_centre = Point(parse(attr[1]), parse(attr[2]));    
    _diameter = parse(attr[3]);
}

void Circle::save(fstream &out)
{
    out << "Circle," << this->_centre.getX() << "," << this->_centre.getY();
    out << "," <<  _diameter << endl;
}

void Circle::prompt(GWindow &gw)
{
    //erase(gw);
    setCentre(gw);
 
    gw.writeText(200, 95, "Diameter: ");
    _diameter = gw.readInt(260, 95);

    gw.writeText(200, 115, "The Circle attributes are set up. Press 'Q' back to main menu");
}

void Circle::show(GWindow &gw)
{
    gw.writeText("    Circle                       ");
    printPoint(gw, this->_centre, "Center");    
    gw.writeText("  Diameter=");
    gw.writeInt(_diameter);
}

int Circle::getDiameter()
{
    return _diameter;
}

void Circle::setDiameter(int diameter)
{
    _diameter = diameter;
}

Circle::Circle(void)
{
}

Circle::~Circle(void)
{
}
