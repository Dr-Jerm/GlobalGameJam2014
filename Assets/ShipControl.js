
var thrustspeed = 500; 
var turnspeed = 50; 
var turnfriction = 2; 
function Update () {
	
	var inputVert = Input.GetAxis("Vertical");
	
	rigidbody.AddRelativeTorque (0,0,(Input.GetAxis("Horizontal"))*thrustspeed*Time.deltaTime);
	rigidbody.AddRelativeForce(0,inputVert*turnspeed*Time.deltaTime,0);
  	
 	
	if (rigidbody.angularVelocity.magnitude > 0) 
	{
    	rigidbody.AddRelativeTorque (rigidbody.angularVelocity.normalized*-turnfriction);
	}    
}