#import "TapdaqStandardAd.h"

extern UIViewController *UnityGetGLViewController();
extern UIView *UnityGetGLView();

static NSString *const kTDUnityBannerStandard = @"TDMBannerStandard";
static NSString *const kTDUnityBannerLarge = @"TDMBannerLarge";
static NSString *const kTDUnityBannerMedium = @"TDMBannerMedium";
static NSString *const kTDUnityBannerFull = @"TDMBannerFull";
static NSString *const kTDUnityBannerLeaderboard = @"TDMBannerLeaderboard";
static NSString *const kTDUnityBannerSmartPortrait = @"TDMBannerSmartPortrait";
static NSString *const kTDUnityBannerSmartLandscape = @"TDMBannerSmartLandscape";

#pragma mark - Banner Native
@interface TapdaqBannerAd ()
@property (strong, nonatomic) UIView *bannerView;
@property (strong, nonatomic) NSArray<NSLayoutConstraint *> *constraintsBanner;
@end

@implementation TapdaqBannerAd

+ (TapdaqBannerAd *)sharedInstance
{
    static dispatch_once_t once;
    static TapdaqBannerAd* sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (void)loadForSize:(NSString *)sizeStr
{
    TDMBannerSize bannerSize = [self bannerSizeFromString:sizeStr];
    
    [[Tapdaq sharedSession] loadBannerWithSize:bannerSize completion:^(UIView *banner) {
        self.bannerView = banner;
    }];
}

- (BOOL)isReady
{
    return self.bannerView != nil;
}

- (void)show
{
    [self show:"bottom"];
}

// position could be "Top" or "Bottom" (ignore case)
- (void)show:(const char *)position
{
    NSString * posString = [NSString stringWithUTF8String: position];
    
    if (self.bannerView != nil) {
        CGSize bannerSize = self.bannerView.frame.size;
        
        UIView *unityView = UnityGetGLView();
        [unityView addSubview:self.bannerView];
        self.bannerView.translatesAutoresizingMaskIntoConstraints = false;
        
        BOOL isTop = [[posString lowercaseString] isEqualToString:@"top"];
        UILayoutGuide *layoutGuide;
        if ([[[UIDevice currentDevice] systemVersion] compare:@"11.0" options:NSNumericSearch] != NSOrderedAscending) {
            layoutGuide = [unityView performSelector:@selector(safeAreaLayoutGuide)];
        } else {
            layoutGuide = unityView.layoutMarginsGuide;
        }
        NSLayoutConstraint *constraintYAnchor = [isTop ? self.bannerView.topAnchor : self.bannerView.bottomAnchor constraintEqualToAnchor:isTop ? layoutGuide.topAnchor : layoutGuide.bottomAnchor];
        
        NSArray *horizontalConstraints = [NSLayoutConstraint constraintsWithVisualFormat:@"H:|->=0-[bannerView(bannerWidth)]->=0-|" options:0 metrics:@{@"bannerWidth" : @(CGRectGetWidth(self.bannerView.frame))} views:@{ @"bannerView" : self.bannerView }];
        NSLayoutConstraint *constraintHeight = [NSLayoutConstraint constraintWithItem:self.bannerView attribute:NSLayoutAttributeHeight relatedBy:NSLayoutRelationEqual toItem:nil attribute:NSLayoutAttributeHeight multiplier:1 constant:bannerSize.height];
        NSLayoutConstraint *constraintCenterX = [NSLayoutConstraint constraintWithItem:self.bannerView attribute:NSLayoutAttributeCenterX relatedBy:NSLayoutRelationEqual toItem:self.bannerView.superview attribute:NSLayoutAttributeCenterX multiplier:1 constant:0];
        NSArray *aggregateConstraints = [horizontalConstraints arrayByAddingObjectsFromArray:@[ constraintYAnchor, constraintHeight, constraintCenterX ]];
        self.constraintsBanner = aggregateConstraints;
        [unityView addConstraints:aggregateConstraints];
        
    }
}

- (void)hide
{
    for (NSLayoutConstraint *constraint in self.constraintsBanner) {
        [constraint setActive:false];
    }
    self.constraintsBanner = nil;
    [self.bannerView removeFromSuperview];
    self.bannerView = nil;
}

- (TDMBannerSize)bannerSizeFromString:(NSString *)sizeStr
{
    TDMBannerSize bannerSize = TDMBannerStandard;
    
    if ([sizeStr isEqualToString:kTDUnityBannerStandard]) {
        bannerSize = TDMBannerStandard;
    } else if ([sizeStr isEqualToString:kTDUnityBannerLarge]) {
        bannerSize = TDMBannerLarge;
    } else if ([sizeStr isEqualToString:kTDUnityBannerMedium]) {
        bannerSize = TDMBannerMedium;
    } else if ([sizeStr isEqualToString:kTDUnityBannerFull]) {
        bannerSize = TDMBannerFull;
    } else if ([sizeStr isEqualToString:kTDUnityBannerLeaderboard]) {
        bannerSize = TDMBannerLeaderboard;
    } else if ([sizeStr isEqualToString:kTDUnityBannerSmartPortrait]) {
        bannerSize = TDMBannerSmartPortrait;
    } else if ([sizeStr isEqualToString:kTDUnityBannerSmartLandscape]) {
        bannerSize = TDMBannerSmartLandscape;
    }
    
    return bannerSize;
}

@end

