//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

void _LaunchMediationDebugger();

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevices,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId);

void _SetDelegate();

bool _IsInitialised();

void _SetUserSubjectToGDPR(int userSubjectToGDPR);
int _UserSubjectToGDPR();

void _SetConsentGiven(bool isConsentGiven);
bool _IsConsentGiven();

void _SetAgeRestrictedUser(bool isAgeRestrictedUser);
bool _IsAgeRestrictedUser();

void _SetAdMobContentRating(const char* rating);
const char* _GetAdMobContentRating();

void _SetUserId(const char* userId);
const char* _GetUserId();

void _SetForwardUserId(bool forwardUserId);
bool _ShouldForwardUserId();

// banner

/**
 * Loads a banner.
 * @param size A string. Must be one of following values: TDMBannerStandard, TDMBannerLarge, TDMBannerMedium, TDMBannerFull, TDMBannerLeaderboard, TDMBannerSmartPortrait, TDMBannerSmartLandscape
 */
void _LoadBannerForSize(const char* sizeChar);

bool _IsBannerReady();

void _ShowBanner(const char* position);

void _HideBanner();

// interstitial

void _LoadInterstitialWithTag(const char *tagChar);

bool _IsInterstitialReadyWithTag(const char *tagChar);

void _ShowInterstitialWithTag(const char* tagChar);

// video

void _LoadVideoWithTag(const char *tagChar);

bool _IsVideoReadyWithTag(const char *tagChar);

void _ShowVideoWithTag(const char* tagChar);

// reward video

void _LoadRewardedVideoWithTag(const char *tagChar);

bool _IsRewardedVideoReadyWithTag(const char *tagChar);

void _ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar);

void _LaunchMediationDebugger();

bool _isEmpty(const char *str);

// stats

void _SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale);

// rewards

const char *_GetRewardId(const char* tag);

@interface TapdaqUnityIOS : NSObject

+ (instancetype)sharedInstance;

- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
          isUserSubjectToGDPR:(int)isUserSubjectToGDPR
               isConsentGiven:(int)isConsentGiven
          isAgeRestrictedUser:(int)isAgeRestrictedUser
                       userId:(NSString*)userId
          shouldForwardUserId:(bool)forwardUserId;
    
- (void) setDelegate;

-(BOOL) IsInitialised;

- (void) setUserSubjectToGDPR:(int)userSubjectToGDPR;

- (int) userSubjectToGDPR;

- (void) setConsentGiven:(BOOL)isConsentGiven;

- (BOOL) isConsentGiven;

- (void) setAgeRestrictedUser:(BOOL)isAgeRestrictedUser;

- (BOOL) isAgeRestrictedUser;

- (void) setAdMobContentRating:(NSString *)rating;

- (NSString *) getAdMobContentRating;

- (void) setUserId:(NSString *)userId;

- (NSString *) getUserId;

- (void) setForwardUserId:(BOOL)forwardUserId;

- (BOOL) shouldForwardUserId;
@end
