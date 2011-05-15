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
 * File:    Rectangle.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

//#pragma once

#include "stdafx.h"
#include "Rectangl.h"

void Rectangl::perform(GWindow &gw)
{
    gw.outlineRectangle(_a.getX(), _a.getY(), _c.getX(), _c.getY());
}

void Rectangl::load(GWindow &gw, vector<string> &attr)
{
    this->_centre = Point(parse(attr[1]), parse(attr[2]));
    _a = Point(parse(attr[3]), parse(attr[4]));
    _b = Point(parse(attr[5]), parse(attr[6]));
    _c = Point(parse(attr[7]), parse(attr[8]));
    _d = Point(parse(attr[9]), parse(attr[10]));
}

void Rectangl::save(fstream &out)
{
    out << "Rectangle," << this->_centre.getX() << "," << this->_centre.getY();
    out << "," << _a.getX() << "," << _a.getY() << "," << _b.getX() << ","; 
    out << _b.getY() << "," << _c.getX() << "," << _c.getY() << ",";
    out << _d.getX() << "," << _d.getY() << endl;
}

void Rectangl::prompt(GWindow &gw)
{
    //erase(gw);
    setCentre(gw);
 
    int x, y;
    gw.writeText(200, 95, "Vertex X: ");
    x = gw.readInt(260, 95);
    gw.writeText(200, 115, "Vertex Y: ");
    y = gw.readInt(260, 115);
    setA(Point(x, y));

    gw.writeText(200, 135, "The Rectangle attributes are set up. Press 'Q' back to main menu");
}

void Rectangl::show(GWindow &gw)
{
    gw.writeText("    Rectangle               ");
    printPoint(gw, this->_centre, "Center");    
    printPoint(gw, _a, " Vertices: A");
    printPoint(gw, _b, "  B");
    printPoint(gw, _c, "  C");   
    printPoint(gw, _d, "  D");
}



//void Rectangle::setD(Point d)
//{
//    _d = d;
//}

Point Rectangl::getD()
{
    return _d;
}

//void Rectangle::setC(Point c)
//{
//    _c = c;
//}

Point Rectangl::getC()
{
    return _c;
}

//void Rectangle::setB(Point b)
//{
//    _b = b;
//}

Point Rectangl::getB()
{
    return _b;
}

void Rectangl::setA(Point a)
{
    _a = a;

    // set opposite vertex
    int x, y;
    x = this->_centre.getX()*2 - _a.getX();
    y = this->_centre.getY()*2 - _a.getY();
    _c = Point(x, y);

    // set other two vertex
    _b = Point(_b.getX(), _a.getY());
    _d = Point(_a.getX(), _b.getY());
}

Point Rectangl::getA()
{
    return _a;
}

Rectangl::Rectangl(Point a, Point b)
{
    _a = a;
    _b = b;

}

Rectangl::Rectangl(void)
{
}

Rectangl::~Rectangl(void)
{
}
