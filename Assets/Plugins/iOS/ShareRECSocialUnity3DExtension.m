//
//  ShareRecSocialUnity3DExtension.m
//  ShareRecGameSample
//
//  Created by vimfung on 14-11-14.
//  Copyright (c) 2014年 掌淘科技. All rights reserved.
//

#import "ShareRecSocialUnity3DExtension.h"
#import "JSONKit.h"
#import <ShareRECSocial/ShareRECSocial.h>

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *	@brief	打开ShareRec社区
     *
     *	@param 	title       分享视频标题
     *  @param  userData    分享视频的用户数据,JSON字符串结构
     *  @param  pageType    社区页面类型
     *  @param  observer    回调对象名称
     */
    extern void __iosShareRECSocialOpen(void *title, void *userData, int pageType, void *observer);
    
#if defined (__cplusplus)
}
#endif

#if defined (__cplusplus)
extern "C" {
#endif
    
    extern void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
    void __iosShareRECSocialOpen(void *title, void *userData, int pageType, void *observer)
    {
        NSString *titleStr = nil;
        if (title)
        {
            titleStr = [NSString stringWithCString:title encoding:NSUTF8StringEncoding];
        }
        
        NSDictionary *userDataDict = nil;
        if (userData)
        {
            NSString *userDataStr = [NSString stringWithCString:userData encoding:NSUTF8StringEncoding];
            userDataDict = [userDataStr objectFromJSONString];
        }
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareRECSocial openByTitle:titleStr userData:userDataDict pageType:pageType onClose:^{
           
            NSDictionary *resultDict = @{@"name" : @"SocialClose"};
            NSString *resultStr = [resultDict JSONString];
            UnitySendMessage([observerStr UTF8String], "shareRECCallback", [resultStr UTF8String]);
            
        }];
    }
    
#if defined (__cplusplus)
}
#endif

@implementation ShareRecSocialUnity3DExtension

@end
