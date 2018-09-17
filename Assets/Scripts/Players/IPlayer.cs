﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer {

    void moveForward(Rigidbody rb, float value);

    void moveRight(Rigidbody rb, float valaue);

    void exitVehicle(Rigidbody rb, GameObject car);

    void useSpecial(Rigidbody rb);

}
