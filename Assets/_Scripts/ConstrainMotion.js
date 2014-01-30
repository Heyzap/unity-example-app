var xMotion : boolean = true;
var yMotion : boolean = true;
var zMotion : boolean = true;

@script AddComponentMenu("Physics/Constrain Motion")
function FixedUpdate () {
	var relativeSpeed : Vector3 = transform.InverseTransformDirection (rigidbody.velocity);
	
	if (!xMotion)
		relativeSpeed.x = 0;
	if (!yMotion)
		relativeSpeed.y = 0;
	if (!zMotion)
		relativeSpeed.z = 0;
		
	rigidbody.AddRelativeForce (-relativeSpeed, ForceMode.VelocityChange);
}

