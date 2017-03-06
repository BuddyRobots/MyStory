//
//  MyControl.m
//  Unity-iPhone
//
//  Created by 王茜 on 1/20/17.
//
//

#import "MyControl.h"
#import <UIKit/UIKit.h>
#import <ShareREC/ShareREC.framework/Headers/ShareREC.h>

@implementation MyControl

@end







#ifdef __cplusplus

extern "C"
{
#endif
    
    
   extern  void _PauseShareREC()
    {
        [ShareREC pauseRecording];
    }
    
    
   extern  void _ResumeShareREC()
    {
        [ShareREC resumeRecording];
    }
    
    
#ifdef __cplusplus
}
#endif
