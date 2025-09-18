/****************************************************************************
**
** Qtitan Library by Developer Machines (Microsoft-Ribbon implementation for Qt.C++)
** 
** Copyright (c) 2009-2014 Developer Machines (http://www.devmachines.com)
**           ALL RIGHTS RESERVED
** 
**  The entire contents of this file is protected by copyright law and
**  international treaties. Unauthorized reproduction, reverse-engineering
**  and distribution of all or any portion of the code contained in this
**  file is strictly prohibited and may result in severe civil and 
**  criminal penalties and will be prosecuted to the maximum extent 
**  possible under the law.
**
**  RESTRICTIONS
**
**  THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED
**  FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE
**  COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE
**  AVAILABLE TO OTHER INDIVIDUALS WITHOUT WRITTEN CONSENT
**  AND PERMISSION FROM DEVELOPER MACHINES
**
**  CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON
**  ADDITIONAL RESTRICTIONS.
**
****************************************************************************/
#ifndef QTN_RIBBONPRIVATE_H
#define QTN_RIBBONPRIVATE_H

#include <QEvent>
#include <QMenu>
#include <QToolButton>
#include <QStyleOption>
#include "QtitanDef.h"

class QEventLoop;

namespace Qtitan
{
    class RibbonGroup;
    class ExWidgetWrapperPrivate;

    /* ExWidgetWrapper */
    class ExWidgetWrapper: public QWidget
    {
        Q_OBJECT
        Q_PROPERTY(QString text READ text WRITE setText)
        Q_PROPERTY(QIcon icon READ icon WRITE setIcon)
    protected:
        explicit ExWidgetWrapper(QWidget* parent, QWidget* widget = Q_NULL);
        virtual ~ExWidgetWrapper();

    public:
        QString text() const;
        void setText(const QString& text);

        void setIcon(const QIcon& icon);
        QIcon icon() const;

        void setAlignWidget(bool align);
        bool alignWidget() const;

        void setLengthLabel(int length);

        QWidget* buddy() const;

        QVariant inputMethodQuery(Qt::InputMethodQuery property) const;

    public:
        virtual QSize sizeHint() const;

    protected:
        virtual void focusInEvent(QFocusEvent* event);
        virtual void focusOutEvent(QFocusEvent* event);
        virtual void paintEvent(QPaintEvent* event);
        virtual void resizeEvent(QResizeEvent* event);
        virtual void inputMethodEvent(QInputMethodEvent* event);
        virtual bool event(QEvent* event);

    protected:
        void updateGeometries();
        void initStyleOption(QStyleOptionFrameV2* option) const;

    protected:
        virtual bool isReadOnly() const;

    private:
        friend class RibbonGroup;
        QTN_DECLARE_PRIVATE(ExWidgetWrapper)
        Q_DISABLE_COPY(ExWidgetWrapper)
    };

    /* RibbonDefaultGroupButton */
    class RibbonDefaultGroupButton : public QToolButton
    {
        Q_OBJECT
    public:
        explicit RibbonDefaultGroupButton(QWidget* p, RibbonGroup* group);
        virtual ~RibbonDefaultGroupButton();

    public:
        bool isShowPopup() const;
        virtual void setVisible(bool visible);

    protected:
        void resetReducedGroup();

    public Q_SLOTS:
        void resetPopopGroup();

    public:
        virtual QSize sizeHint() const;

    protected:
        virtual bool event(QEvent* event);
        virtual void paintEvent(QPaintEvent* event);
        virtual void mousePressEvent(QMouseEvent* event);
        virtual void mouseReleaseEvent(QMouseEvent* event);

    private:
        bool m_hasPopup;
        QEventLoop* m_eventLoop;
        RibbonGroup* m_ribbonGroup;

    private:
        Q_DISABLE_COPY(RibbonDefaultGroupButton)
    };

    /* RibbonGroupOption */
    class RibbonGroupOption: public QToolButton
    {
        Q_OBJECT
    public:
        RibbonGroupOption(QWidget* parent);
        virtual ~RibbonGroupOption();

    public:
        QString text() const;

    public:
        virtual QSize sizeHint() const;

    protected:
        virtual void paintEvent(QPaintEvent* event);
        virtual void actionEvent(QActionEvent* event);
    private:
        Q_DISABLE_COPY(RibbonGroupOption)
    };


    /* RibbonGroupScroll */
    class RibbonGroupScroll : public QToolButton
    {
        Q_OBJECT
    public:
        RibbonGroupScroll(QWidget* parent, bool scrollLeft);
        virtual ~RibbonGroupScroll();

    protected:
        virtual void paintEvent(QPaintEvent* event);
    protected:
        bool m_scrollLeft;
    };


    class RibbonKeyTip;

    /* KeyTipEvent */
    class KeyTipEvent : public QEvent
    {
    public:
        //! Constructor for the event.
        explicit KeyTipEvent(RibbonKeyTip* kTip);

    public:
        RibbonKeyTip* getKeyTip() const;

    public:
        // Returns the event type/number for KeyTipEvent.
        // The type is registered on first access. Use this to detect incoming
        static QEvent::Type eventNumber();
    protected:
        RibbonKeyTip* keyTip;
    };

    /* ShowKeyTipEvent */
    class ShowKeyTipEvent : public QEvent
    {
    public:
        //! Constructor for the event.
        ShowKeyTipEvent(QWidget* w);

    public:
        QWidget* getWidget() const;

    public:
        // Returns the event type/number for ShowKeyTipEvent.
        // The type is registered on first access. Use this to detect incoming
        static QEvent::Type eventNumber();
    protected:
        QWidget* widget;
    };

    /* HideKeyTipEvent */
    class HideKeyTipEvent : public QEvent
    {
    public:
        //! Constructor for the event.
        HideKeyTipEvent();
    public:
        // Returns the event type/number for ShowKeyTipEvent.
        // The type is registered on first access. Use this to detect incoming
        static QEvent::Type eventNumber();
    };

    /* MinimizedEvent */
    class MinimizedEvent : public QEvent
    {
    public:
        //! Constructor for the event.
        MinimizedEvent();
    public:
        // Returns the event type/number for ShowKeyTipEvent.
        // The type is registered on first access. Use this to detect incoming
        static QEvent::Type eventNumber();
    };

}; //namespace Qtitan


#endif // QTN_RIBBONPRIVATE_H


