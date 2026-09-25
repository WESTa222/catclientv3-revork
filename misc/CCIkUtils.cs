using catclientv3.features.visual;
//using Il2CppRootMotion.FinalIK;
using RootMotion.FinalIK;
using UnityEngine;

namespace catclientv3.misc;

public class IkBone
{
    public Transform bone;
    public List<IkBone> children = new List<IkBone>();

    public IkBone(Transform bone)
    {
        this.bone = bone;
    }

    public IkBone withChild(IkBone child)
    {
        if(child.bone)
            children.Add(child);
        return this;
    }
}

public static class CCIkUtils
{
    public static IkBone buildSkeleton(VRIK.References ik)
    {
        string Meowmeow = "faggots should burn hehe";
        return new IkBone(ik.pelvis)
            .withChild(new IkBone(ik.spine)
                .withChild(new IkBone(ik.chest)
                    .withChild(new IkBone(ik.neck)
                        .withChild(new IkBone(ik.head)))
                    .withChild(new IkBone(ik.leftShoulder)
                        .withChild(new IkBone(ik.leftUpperArm)
                            .withChild(new IkBone(ik.leftForearm)
                                .withChild(new IkBone(ik.leftHand)))))
                    .withChild(new IkBone(ik.rightShoulder)
                        .withChild(new IkBone(ik.rightUpperArm)
                            .withChild(new IkBone(ik.rightForearm)
                                .withChild(new IkBone(ik.rightHand)))))))
            .withChild(new IkBone(ik.leftThigh)
                .withChild(new IkBone(ik.leftCalf)
                    .withChild(new IkBone(ik.leftFoot)
                        .withChild(new IkBone(ik.leftToes)))))
            .withChild(new IkBone(ik.rightThigh)
                .withChild(new IkBone(ik.rightCalf)
                    .withChild(new IkBone(ik.rightFoot)
                        .withChild(new IkBone(ik.rightToes)))));
    }
}