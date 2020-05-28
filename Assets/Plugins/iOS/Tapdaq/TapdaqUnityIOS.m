//
//  TapdaqUnityIOS.m
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import "TapdaqUnityIOS.h"
#import "TapdaqDelegates.h"
#import "TapdaqStandardAd.h"

static NSString *const kTapdaqLogPrefix = @"[TapdaqUnity]";

// Helper method to create C string copy
char* makeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevicesChar,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId) {
    
    bool isValid = true;
    
    if (_isEmpty(appIdChar)) {
        NSLog(@"%@ iOS App ID is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(clientKeyChar)) {
        NSLog(@"%@ iOS Client Key is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Tapdaq did not initialise", kTapdaqLogPrefix);
        return;
    }
    
    NSString *appId = [[NSString stringWithUTF8String:appIdChar] copy];
    NSString *clientKey = [[NSString stringWithUTF8String:clientKeyChar] copy];
    NSString *testDevices = [[NSString stringWithUTF8String:testDevicesChar] copy];
    NSString *version = [[NSString stringWithUTF8String:pluginVersion] copy];
    
    NSString *userId = nil;
    
    if (!_isEmpty(userIdChar)) {
        userId = [[NSString stringWithUTF8String:userIdChar] copy];
    }
    
    [[TapdaqUnityIOS sharedInstance] initWithApplicationId:appId
                                                 clientKey:clientKey
                                               testDevices:testDevices
                                               isDebugMode:isDebugMode
                                             autoReloadAds:autoReloadAds
                                             pluginVersion:version
                                       isUserSubjectToGDPR:isUserSubjectToGDPR
                                            isConsentGiven:isConsentGiven
                                       isAgeRestrictedUser:isAgeRestrictedUser
                                                    userId:userId
                                       shouldForwardUserId:forwardUserId];
    
}

void _SetDelegate() {
    [[TapdaqUnityIOS sharedInstance] setDelegate];
}

bool _IsInitialised() {
    return  [[TapdaqUnityIOS sharedInstance] IsInitialised];
}

void _SetUserSubjectToGDPR(int userSubjectToGDPR) {
    [[TapdaqUnityIOS sharedInstance] setUserSubjectToGDPR:userSubjectToGDPR];
}

int _UserSubjectToGDPR() {
    return (int)[[TapdaqUnityIOS sharedInstance] userSubjectToGDPR];
}

void _SetConsentGiven(bool isConsentGiven) {
    [[TapdaqUnityIOS sharedInstance] setConsentGiven:isConsentGiven];
}

bool _IsConsentGiven() {
    return (bool)[[TapdaqUnityIOS sharedInstance] isConsentGiven];
}

void _SetAgeRestrictedUser(bool isAgeRestrictedUser) {
    [[TapdaqUnityIOS sharedInstance] setAgeRestrictedUser:isAgeRestrictedUser];
}

bool _IsAgeRestrictedUser() {
    return (bool)[[TapdaqUnityIOS sharedInstance] isAgeRestrictedUser];
}

void _SetAdMobContentRating(const char * rating) {
    NSString *ratingStr = [[NSString stringWithUTF8String:rating] copy];
    [[TapdaqUnityIOS sharedInstance] setAdMobContentRating:ratingStr];
}

const char * _GetAdMobContentRating() {
    NSString * ratingStr = [[[TapdaqUnityIOS sharedInstance] getAdMobContentRating] copy];
    return makeStringCopy([ratingStr UTF8String]);
}

void _SetUserId(const char * userId) {
    NSString *userStr = nil;
    if (!_isEmpty(userId)) {
        userStr = [[NSString stringWithUTF8String:userId] copy];
    }
    [[TapdaqUnityIOS sharedInstance] setUserId:userStr];
}

const char * _GetUserId() {
    NSString * userIdStr = [[[TapdaqUnityIOS sharedInstance] getUserId] copy];
    return makeStringCopy([userIdStr UTF8String]);
}

void _SetForwardUserId(bool forwardUserId) {
    [[TapdaqUnityIOS sharedInstance] setForwardUserId:forwardUserId];
}

bool _ShouldForwardUserId() {
    return (bool)[[TapdaqUnityIOS sharedInstance] shouldForwardUserId];
}

#pragma mark - Banner (Bridge)

void _LoadBannerForSize(const char* sizeChar) {
    
    if (_isEmpty(sizeChar)) {
        NSLog(@"%@ No banner size specified, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *sizeStr = [[NSString stringWithUTF8String:sizeChar] copy];
    
    [[TapdaqBannerAd sharedInstance] loadForSize:sizeStr];
    
}

bool _IsBannerReady() {
    
    return (bool) [[TapdaqBannerAd sharedInstance] isReady];
    
}

void _ShowBanner(const char* position) {
    
    if (_isEmpty(position)) {
        NSLog(@"%@ No banner position given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    
    [[TapdaqBannerAd sharedInstance] show: position];
    
}

void _HideBanner() {
    
    [[TapdaqBannerAd sharedInstance] hide];
    
}

#pragma mark - Interstitial (Bridge)

void _LoadInterstitialWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsInterstitialReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if interstitial is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqInterstitialAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowInterstitialWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Video (Bridge)

void _LoadVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowVideoWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Rewarded Video (Bridge)

void _LoadRewardedVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqRewardedVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsRewardedVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if rewarded video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqRewardedVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    NSString *hashedUserId = (hashedUserIdChar != nil ? [NSString stringWithUTF8String:hashedUserIdChar] : nil);
    
    [[TapdaqRewardedVideoAd sharedInstance] showForPlacementTag:tag withHashedUserId:hashedUserId];
    
}

#pragma mart - Stats

void _SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale) {
    NSString *transactionIdString;
    NSString *productIdString;
    NSString *currencyString;
    NSString *nameString;
    NSString *localeString;
    
    if (transationId != NULL) {
        transactionIdString = [NSString stringWithUTF8String:transationId];
    }
    
    if (productId != NULL) {
        productIdString = [NSString stringWithUTF8String:productId];
    }
    
    if (currency != NULL) {
        currencyString = [NSString stringWithUTF8String:currency];
    }
    
    if (name != NULL) {
        nameString = [NSString stringWithUTF8String:name];
    }
    
    if (locale != NULL) {
        localeString = [NSString stringWithUTF8String:locale];
    }
    
    [[Tapdaq sharedSession] trackPurchaseForProductName:nameString
                                                  price:price
                                            priceLocale:localeString
                                               currency:currencyString
                                          transactionId:transactionIdString
                                              productId:productIdString];
}

const char *_GetRewardId(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    NSString *rewardId = [[Tapdaq sharedSession] rewardIdForPlacementTag:placementTag];
    return [rewardId cStringUsingEncoding:NSUTF8StringEncoding];
}

void _LaunchMediationDebugger() {
    [[Tapdaq sharedSession] presentDebugViewController];
}

bool _isEmpty(const char* str) {
    return str == NULL;
}

@implementation TapdaqUnityIOS

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

+ (void)load
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(createPlugin:) name:UIApplicationDidFinishLaunchingNotification object:nil];
}

+ (void)createPlugin:(NSNotification *)notification
{
    [TapdaqUnityIOS sharedInstance];
}

// Init
//Configure Tapdaq with credentials and ad settings
- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
          isUserSubjectToGDPR:(int)isUserSubjectToGDPR
               isConsentGiven:(int)isConsentGiven
          isAgeRestrictedUser:(int)isAgeRestrictedUser
                       userId:(NSString *)userId
                shouldForwardUserId:(bool)forwardUserId
{
    TDProperties *properties = [[TDProperties alloc] init];
    [properties setPluginVersion:pluginVersion];
    properties.isDebugEnabled = isDebugMode;
    properties.logLevel = isDebugMode ? TDLogLevelDebug : TDLogLevelInfo;
    [properties setAutoReloadAds:autoReloadAds];
    properties.userId = userId;
    properties.forwardUserId = forwardUserId;

    if (isConsentGiven != 2) {
        properties.isConsentGiven = (BOOL)isConsentGiven;
    }
    if (isAgeRestrictedUser != 2) {
        properties.isAgeRestrictedUser = isAgeRestrictedUser;
    }
    properties.userSubjectToGDPR = isUserSubjectToGDPR;
    
    [self setTestDevices: testDevices toProperties:properties];
    
    [[Tapdaq sharedSession] setApplicationId:appID
                                   clientKey:clientKey
                                  properties:properties];
    
    [[Tapdaq sharedSession] launch];
}
    
- (void) setDelegate {
    [[Tapdaq sharedSession] setDelegate: [TapdaqDelegates sharedInstance]];
}

- (BOOL) IsInitialised
{
    return [[Tapdaq sharedSession] isInitialised];
}

- (void) setUserSubjectToGDPR:(int)userSubjectToGDPR {
    [[Tapdaq sharedSession] setUserSubjectToGDPR:userSubjectToGDPR];
}

- (int) userSubjectToGDPR {
    return (int)[[Tapdaq sharedSession] userSubjectToGDPR];
}

- (void) setConsentGiven:(BOOL)isConsentGiven
{
    [[Tapdaq sharedSession] setIsConsentGiven:isConsentGiven];
}

- (BOOL) isConsentGiven
{
    return [[Tapdaq sharedSession] isConsentGiven];
}

- (void) setAgeRestrictedUser:(BOOL)isAgeRestrictedUser
{
    [[Tapdaq sharedSession] setIsAgeRestrictedUser:isAgeRestrictedUser];
}

- (BOOL) isAgeRestrictedUser
{
    return [[Tapdaq sharedSession] isAgeRestrictedUser];
}

- (void) setAdMobContentRating:(NSString*)rating
{
    [[Tapdaq sharedSession] setAdMobContentRating:rating];
}

- (NSString*) getAdMobContentRating
{
    return [[Tapdaq sharedSession] adMobContentRating];
}

- (void) setUserId:(NSString*)userId
{
     [[Tapdaq sharedSession] setUserId:userId];
}

- (NSString*) getUserId
{
    return [[Tapdaq sharedSession] userId];
}

- (void) setForwardUserId:(BOOL)forwardUserId
{
   [[Tapdaq sharedSession] setForwardUserId:forwardUserId];
}

- (BOOL) shouldForwardUserId
{
   return [[Tapdaq sharedSession] forwardUserId];
}

- (void) setTestDevices:(NSString *)testDevicesJson toProperties:(TDProperties *)properties
{
    NSData *data = [testDevicesJson dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSDictionary *testDevicesDictionary = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
    
    if(error == nil) {
        NSArray* amArray = testDevicesDictionary[@"adMobDevices"];
        NSArray* fbArray = testDevicesDictionary[@"facebookDevices"];
        
        if(amArray != nil) {
            TDTestDevices *amTestDevices = [[TDTestDevices alloc] initWithNetwork:@"admob" testDevices:amArray];
            [properties registerTestDevices: amTestDevices];
        }
        
        if(fbArray != nil) {
            TDTestDevices *fbTestDevices = [[TDTestDevices alloc] initWithNetwork:@"facebook" testDevices:fbArray];
            [properties registerTestDevices: fbTestDevices];
        }
    }
}

@end
